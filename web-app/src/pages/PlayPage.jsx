import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../services/api";
import { useAuth } from "../context/AuthContext";

export default function PlayPage() {
  const { packId } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [puzzle, setPuzzle] = useState(null);
  const [images, setImages] = useState([]);
  const [guess, setGuess] = useState("");
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [feedback, setFeedback] = useState(null);
  const [userStats, setUserStats] = useState({ totalScore: 0, puzzlesSolved: 0 });
  const [error, setError] = useState(null);

  // Fetch next puzzle
  const fetchNextPuzzle = async () => {
    try {
      setLoading(true);
      setFeedback(null);
      setGuess("");
      setError(null);

      const response = await api.get("/puzzles/next", {
        params: {
          packId: packId,
          userId: user?.id || ""
        }
      });

      console.log("Full API response:", response.data);
      console.log("Images received:", response.data.images);
      console.log("Number of images:", response.data.images?.length);

      setPuzzle(response.data.puzzleId);
      setImages(response.data.images?.map(img => ({
        ...img,
        url: img.url?.startsWith("http")
          ? img.url
          : `http://localhost:5208${img.url}`
      })) || []);
    } catch (err) {
      console.error("Error fetching puzzle:", err.response?.data || err.message);
      console.log("Response status:", err.response?.status);
      console.log("Response data:", err.response?.data);
      setError(`Failed to load puzzle: ${err.response?.status || err.message}`);
    } finally {
      setLoading(false);
    }
  };

  // Fetch user stats
  const fetchUserStats = async () => {
    if (!user?.id) return;

    try {
      const response = await api.get(`/game/profile/${user.id}`);
      setUserStats({
        totalScore: response.data.totalScore,
        puzzlesSolved: response.data.puzzlesSolved
      });
    } catch (err) {
      console.error("Error fetching user stats:", err);
    }
  };

  // Load initial puzzle and stats
  useEffect(() => {
    if (!user?.id) {
      console.log("No user ID available:", user);
      return;
    }
    console.log("User ID:", user.id, "Pack ID:", packId);
    fetchNextPuzzle();
    fetchUserStats();
  }, [user?.id, packId]);

  // Handle guess submission
  const handleSubmitGuess = async (e) => {
    e.preventDefault();

    if (!guess.trim()) {
      setError("Please enter a guess");
      return;
    }

    try {
      setSubmitting(true);
      setError(null);

      const response = await api.post("/game/submit", {
        puzzleId: puzzle,
        userId: user?.id || "",
        guess: guess.trim()
      });

      const { correct, score } = response.data;
      setFeedback({ correct, score });

      // Update stats if correct
      if (correct) {
        setUserStats(prev => ({
          totalScore: prev.totalScore + score,
          puzzlesSolved: prev.puzzlesSolved + 1
        }));
      }

      // Auto-load next puzzle after feedback delay (for both correct and incorrect)
      setTimeout(async () => {
        setFeedback(null);
        await fetchNextPuzzle();
        if (correct) {
          await fetchUserStats();
        }
      }, 2000);
    } catch (err) {
      setError("Failed to submit guess. Please try again.");
      console.error("Error submitting guess:", err);
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
        fontSize: "18px",
        color: "#666"
      }}>
        Loading puzzle...
      </div>
    );
  }

  if (error && !feedback) {
    return (
      <div style={{
        textAlign: "center",
        padding: "40px",
        color: "#dc3545",
        fontSize: "16px"
      }}>
        <p>{error}</p>
        <button
          onClick={() => navigate("/packs")}
          style={{
            padding: "10px 20px",
            backgroundColor: "#007bff",
            color: "white",
            border: "none",
            borderRadius: "4px",
            cursor: "pointer",
            fontSize: "14px"
          }}
        >
          Back to Packs
        </button>
      </div>
    );
  }

  return (
    <div style={{ padding: "20px", maxWidth: "900px", margin: "0 auto" }}>
      {/* Profile Progress */}
      <div style={{
        display: "flex",
        justifyContent: "space-between",
        marginBottom: "30px",
        padding: "15px",
        backgroundColor: "#f8f9fa",
        borderRadius: "8px"
      }}>
        <div>
          <p style={{ margin: "0 0 5px 0", color: "#666", fontSize: "14px" }}>
            Total Score
          </p>
          <p style={{ margin: 0, fontSize: "24px", fontWeight: "bold", color: "#333" }}>
            {userStats.totalScore}
          </p>
        </div>
        <div>
          <p style={{ margin: "0 0 5px 0", color: "#666", fontSize: "14px" }}>
            Puzzles Solved
          </p>
          <p style={{ margin: 0, fontSize: "24px", fontWeight: "bold", color: "#333" }}>
            {userStats.puzzlesSolved}
          </p>
        </div>
      </div>

      {/* Image Grid */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(2, 1fr)",
        gap: "15px",
        marginBottom: "30px"
      }}>
        {images.map((image) => (
          <div
            key={image.id}
            style={{
              aspectRatio: "1 / 1",
              overflow: "hidden",
              borderRadius: "8px",
              backgroundColor: "#e9ecef"
            }}
          >
            <img
              src={image.url}
              alt={`Image ${image.position + 1}`}
              onError={(e) => {
                e.currentTarget.onerror = null;
                e.currentTarget.src = "https://via.placeholder.com/500x500?text=Image+Unavailable";
              }}
              style={{
                width: "100%",
                height: "100%",
                objectFit: "cover"
              }}
            />
          </div>
        ))}
      </div>

      {/* Feedback or Input */}
      {feedback ? (
        <div style={{
          textAlign: "center",
          padding: "30px",
          backgroundColor: feedback.correct ? "#d4edda" : "#f8d7da",
          border: `2px solid ${feedback.correct ? "#28a745" : "#dc3545"}`,
          borderRadius: "8px",
          marginBottom: "20px"
        }}>
          <p style={{ margin: "0 0 10px 0", fontSize: "24px", fontWeight: "bold" }}>
            {feedback.correct ? "✅ Correct!" : "❌ Wrong"}
          </p>
          {feedback.correct && (
            <p style={{ margin: "0 0 15px 0", fontSize: "18px", color: "#155724" }}>
              +{feedback.score} points!
            </p>
          )}
          <p style={{ margin: "0", fontSize: "16px", color: "#666" }}>
            Loading next puzzle...
          </p>
        </div>
      ) : (
        <form onSubmit={handleSubmitGuess} style={{ marginBottom: "20px" }}>
          <div style={{ marginBottom: "15px" }}>
            <label style={{
              display: "block",
              marginBottom: "8px",
              fontSize: "16px",
              fontWeight: "bold",
              color: "#333"
            }}>
              What do these images have in common?
            </label>
            <input
              type="text"
              value={guess}
              onChange={(e) => setGuess(e.target.value)}
              placeholder="Enter your guess..."
              disabled={submitting}
              style={{
                width: "100%",
                padding: "12px",
                fontSize: "16px",
                border: "2px solid #ddd",
                borderRadius: "4px",
                boxSizing: "border-box",
                disabled: submitting ? 0.7 : 1
              }}
            />
          </div>
          <button
            type="submit"
            disabled={submitting}
            style={{
              width: "100%",
              padding: "12px",
              backgroundColor: submitting ? "#ccc" : "#28a745",
              color: "white",
              border: "none",
              borderRadius: "4px",
              cursor: submitting ? "not-allowed" : "pointer",
              fontSize: "16px",
              fontWeight: "bold"
            }}
          >
            {submitting ? "Submitting..." : "Submit"}
          </button>
        </form>
      )}

      {error && feedback && (
        <div style={{
          padding: "12px",
          backgroundColor: "#fff3cd",
          border: "1px solid #ffc107",
          borderRadius: "4px",
          color: "#856404",
          marginBottom: "20px"
        }}>
          {error}
        </div>
      )}

      {/* Back Button */}
      <button
        onClick={() => navigate("/packs")}
        style={{
          padding: "10px 20px",
          backgroundColor: "#6c757d",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: "pointer",
          fontSize: "14px"
        }}
      >
        Back to Packs
      </button>
    </div>
  );
}
