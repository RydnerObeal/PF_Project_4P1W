import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useEffect, useState } from "react";
import api from "../services/api";

export default function Home() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [userStats, setUserStats] = useState({ totalScore: 0, puzzlesSolved: 0 });
  const [packStats, setPackStats] = useState([]);
  const [topScorer, setTopScorer] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!user?.id) return;

    const fetchUserData = async () => {
      try {
        setLoading(true);

        // Fetch current user's total stats
        const profileResponse = await api.get(`/game/profile/${user.id}`);
        setUserStats({
          totalScore: profileResponse.data.totalScore,
          puzzlesSolved: profileResponse.data.puzzlesSolved,
          totalAttempts: profileResponse.data.totalAttempts
        });

        // Fetch top scorer (global leaderboard)
        try {
          const topScorerResponse = await api.get("/game/top-scorer");
          setTopScorer(topScorerResponse.data);
        } catch (err) {
          console.log("No top scorer data available");
        }
      } catch (error) {
        console.error("Failed to fetch user data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, [user?.id]);

  const handlePlayClick = () => {
    navigate("/packs");
  };

  const handleProfileClick = () => {
    navigate("/profile");
  };

  if (loading) {
    return (
      <div style={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        color: "#cbbfe4",
        fontSize: "16px"
      }}>
        Loading your profile...
      </div>
    );
  }

  return (
    <div style={{
      width: "min(1120px, calc(100% - 24px))",
      margin: "0 auto 32px",
      padding: "8px"
    }}>
      {/* Welcome Header */}
      <div style={{
        marginBottom: "32px",
        padding: "32px",
        borderRadius: "24px",
        background: "rgba(255, 255, 255, 0.06)",
        border: "1px solid rgba(255, 255, 255, 0.12)",
        boxShadow: "0 20px 50px rgba(8, 11, 28, 0.16)"
      }}>
        <div style={{ display: "flex", alignItems: "center", gap: "16px", marginBottom: "16px" }}>
          <span style={{ fontSize: "48px" }}>👋</span>
          <div>
            <h1 style={{
              margin: 0,
              fontSize: "32px",
              fontWeight: "700",
              color: "#f2ecff"
            }}>Welcome back, {user?.email}</h1>
            <p style={{
              margin: "8px 0 0 0",
              fontSize: "14px",
              color: "#cbbfe4"
            }}>Ready to solve some puzzles and earn points?</p>
          </div>
        </div>
      </div>

      {/* Your Stats */}
      <div style={{
        marginBottom: "32px",
        padding: "24px",
        borderRadius: "24px",
        background: "rgba(255, 255, 255, 0.06)",
        border: "1px solid rgba(255, 255, 255, 0.12)",
        boxShadow: "0 20px 50px rgba(8, 11, 28, 0.16)"
      }}>
        <h2 style={{
          margin: "0 0 24px 0",
          fontSize: "20px",
          fontWeight: "700",
          color: "#f2ecff"
        }}>📊 Your Score</h2>
        
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
          gap: "20px"
        }}>
          {/* Total Score Card */}
          <div style={{
            padding: "24px",
            borderRadius: "16px",
            background: "linear-gradient(135deg, rgba(139, 92, 246, 0.15) 0%, rgba(194, 136, 255, 0.1) 100%)",
            border: "1px solid rgba(194, 136, 255, 0.3)",
            textAlign: "center"
          }}>
            <p style={{
              margin: "0 0 12px 0",
              fontSize: "12px",
              fontWeight: "600",
              color: "#cbbfe4",
              textTransform: "uppercase",
              letterSpacing: "1px"
            }}>Total Points</p>
            <p style={{
              margin: 0,
              fontSize: "48px",
              fontWeight: "700",
              color: "#d8b4fe"
            }}>{userStats.totalScore}</p>
          </div>

          {/* Puzzles Solved Card */}
          <div style={{
            padding: "24px",
            borderRadius: "16px",
            background: "linear-gradient(135deg, rgba(168, 85, 247, 0.15) 0%, rgba(217, 70, 239, 0.1) 100%)",
            border: "1px solid rgba(217, 70, 239, 0.3)",
            textAlign: "center"
          }}>
            <p style={{
              margin: "0 0 12px 0",
              fontSize: "12px",
              fontWeight: "600",
              color: "#cbbfe4",
              textTransform: "uppercase",
              letterSpacing: "1px"
            }}>Puzzles Solved</p>
            <p style={{
              margin: 0,
              fontSize: "48px",
              fontWeight: "700",
              color: "#f472b6"
            }}>{userStats.puzzlesSolved}</p>
          </div>
        </div>
      </div>

      {/* Top Scorer */}
      {topScorer && topScorer.userId !== user?.id && (
        <div style={{
          marginBottom: "32px",
          padding: "24px",
          borderRadius: "24px",
          background: "rgba(255, 215, 0, 0.08)",
          border: "1px solid rgba(255, 215, 0, 0.3)",
          boxShadow: "0 20px 50px rgba(8, 11, 28, 0.16)"
        }}>
          <h2 style={{
            margin: "0 0 16px 0",
            fontSize: "20px",
            fontWeight: "700",
            color: "#f2ecff"
          }}>🏆 Top Player</h2>
          <div style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            padding: "16px",
            borderRadius: "12px",
            background: "rgba(255, 255, 255, 0.04)",
            border: "1px solid rgba(255, 255, 255, 0.08)"
          }}>
            <div>
              <p style={{
                margin: "0 0 4px 0",
                fontSize: "12px",
                color: "#cbbfe4"
              }}>Player</p>
              <p style={{
                margin: 0,
                fontSize: "16px",
                fontWeight: "600",
                color: "#f2ecff"
              }}>{topScorer.email}</p>
            </div>
            <div style={{ textAlign: "right" }}>
              <p style={{
                margin: "0 0 4px 0",
                fontSize: "12px",
                color: "#cbbfe4"
              }}>Score</p>
              <p style={{
                margin: 0,
                fontSize: "28px",
                fontWeight: "700",
                color: "#ffd700"
              }}>{topScorer.totalScore}</p>
            </div>
          </div>
        </div>
      )}

      {/* CTA Buttons */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "1fr 1fr",
        gap: "16px"
      }}>
        <button
          onClick={handlePlayClick}
          style={{
            padding: "16px 24px",
            borderRadius: "16px",
            background: "linear-gradient(135deg, #8b5cf6 0%, #c084fc 100%)",
            color: "#ffffff",
            border: "1px solid rgba(255, 255, 255, 0.2)",
            fontSize: "16px",
            fontWeight: "700",
            cursor: "pointer",
            transition: "all 0.2s ease",
            boxShadow: "0 12px 28px rgba(139, 92, 246, 0.18)"
          }}
          onMouseEnter={(e) => {
            e.target.style.transform = "translateY(-2px)";
            e.target.style.boxShadow = "0 18px 34px rgba(139, 92, 246, 0.3)";
          }}
          onMouseLeave={(e) => {
            e.target.style.transform = "translateY(0)";
            e.target.style.boxShadow = "0 12px 28px rgba(139, 92, 246, 0.18)";
          }}
        >
          🎮 Play Puzzle Packs
        </button>

        <button
          onClick={handleProfileClick}
          style={{
            padding: "16px 24px",
            borderRadius: "16px",
            background: "rgba(255, 255, 255, 0.08)",
            color: "#f2ecff",
            border: "1px solid rgba(255, 255, 255, 0.2)",
            fontSize: "16px",
            fontWeight: "700",
            cursor: "pointer",
            transition: "all 0.2s ease",
            boxShadow: "0 12px 28px rgba(8, 11, 28, 0.16)"
          }}
          onMouseEnter={(e) => {
            e.target.style.background = "rgba(255, 255, 255, 0.12)";
            e.target.style.transform = "translateY(-2px)";
            e.target.style.boxShadow = "0 18px 34px rgba(8, 11, 28, 0.25)";
          }}
          onMouseLeave={(e) => {
            e.target.style.background = "rgba(255, 255, 255, 0.08)";
            e.target.style.transform = "translateY(0)";
            e.target.style.boxShadow = "0 12px 28px rgba(8, 11, 28, 0.16)";
          }}
        >
          📊 View Profile
        </button>
      </div>
    </div>
  );
}