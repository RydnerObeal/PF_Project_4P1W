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
  const [packStats, setPackStats] = useState({ packScore: 0, packPuzzlesSolved: 0 });
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
      const processedImages = response.data.images?.map(img => {
        const fullUrl = img.url?.startsWith("http")
          ? img.url
          : `http://localhost:5208${img.url}`;
        console.log("Processing image:", img.url, "->", fullUrl);
        return {
          ...img,
          url: fullUrl
        };
      }) || [];
      console.log("Processed images:", processedImages);
      setImages(processedImages);
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
      const globalResponse = await api.get(`/game/profile/${user.id}`);
      console.log("Global stats response:", globalResponse.data);
      setUserStats({
        totalScore: globalResponse.data.totalScore,
        puzzlesSolved: globalResponse.data.puzzlesSolved
      });

      // Fetch pack-specific stats
      const packResponse = await api.get(`/game/profile/${user.id}/pack/${packId}`);
      console.log("Pack stats response:", packResponse.data);
      setPackStats({
        packScore: packResponse.data.packScore || 0,
        packPuzzlesSolved: packResponse.data.packPuzzlesSolved || 0
      });
    } catch (err) {
      console.error("Error fetching user stats:", err);
      console.error("Error details:", err.response?.data);
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
        packId: packId,
        guess: guess.trim().toLowerCase()
      });

      const { correct, score } = response.data;
      setFeedback({ correct, score });

      // Update stats if correct
      if (correct) {
        setUserStats(prev => ({
          totalScore: prev.totalScore + score,
          puzzlesSolved: prev.puzzlesSolved + 1
        }));
        setPackStats(prev => ({
          packScore: prev.packScore + score,
          packPuzzlesSolved: prev.packPuzzlesSolved + 1
        }));
      }

      // Auto-load next puzzle after feedback delay (for both correct and incorrect)
      setTimeout(async () => {
        setFeedback(null);
        if (correct) {
          // Refetch all stats before loading next puzzle
          await fetchUserStats();
        }
        await fetchNextPuzzle();
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
      {/* Stats - Pack and Total */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(2, 1fr)",
        gap: "15px",
        marginBottom: "30px"
      }}>
        {/* Pack Stats */}
        <div style={{
          padding: "20px",
          backgroundColor: "#e8f4f8",
          borderRadius: "12px",
          border: "2px solid #0099cc"
        }}>
          <p style={{ margin: "0 0 8px 0", color: "#0066aa", fontSize: "12px", fontWeight: "600" }}>
            THIS PACK
          </p>
          <div style={{ display: "flex", justifyContent: "space-between" }}>
            <div>
              <p style={{ margin: "0 0 4px 0", color: "#666", fontSize: "13px" }}>High Score</p>
              <p style={{ margin: 0, fontSize: "28px", fontWeight: "bold", color: "#0099cc" }}>
                {packStats.packScore}
              </p>
            </div>
            <div>
              <p style={{ margin: "0 0 4px 0", color: "#666", fontSize: "13px" }}>Solved</p>
              <p style={{ margin: 0, fontSize: "28px", fontWeight: "bold", color: "#0099cc" }}>
                {packStats.packPuzzlesSolved}
              </p>
            </div>
          </div>
        </div>

        {/* Total Stats */}
        <div style={{
          padding: "20px",
          backgroundColor: "#f0e8f8",
          borderRadius: "12px",
          border: "2px solid #9933ff"
        }}>
          <p style={{ margin: "0 0 8px 0", color: "#6600cc", fontSize: "12px", fontWeight: "600" }}>
            TOTAL
          </p>
          <div style={{ display: "flex", justifyContent: "space-between" }}>
            <div>
              <p style={{ margin: "0 0 4px 0", color: "#666", fontSize: "13px" }}>Total Score</p>
              <p style={{ margin: 0, fontSize: "28px", fontWeight: "bold", color: "#9933ff" }}>
                {userStats.totalScore}
              </p>
            </div>
            <div>
              <p style={{ margin: "0 0 4px 0", color: "#666", fontSize: "13px" }}>Solved</p>
              <p style={{ margin: 0, fontSize: "28px", fontWeight: "bold", color: "#9933ff" }}>
                {userStats.puzzlesSolved}
              </p>
            </div>
          </div>
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
              width: "300px",
              height: "300px",
              overflow: "hidden",
              borderRadius: "8px",
              backgroundColor: "#e9ecef"
            }}
          >
            <img
              src={image.url}
              alt={`Image ${image.position + 1}`}
              onLoad={() => console.log("Image loaded successfully:", image.url)}
              onError={(e) => {
                console.error("Image failed to load:", image.url, e);
                e.currentTarget.onerror = null;
                e.currentTarget.src = "https://via.placeholder.com/300x300?text=Image+Unavailable";
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
