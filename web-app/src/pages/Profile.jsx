import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import api from "../services/api";

export default function Profile() {
  const { user } = useAuth();
  const [stats, setStats] = useState({ totalScore: 0, puzzlesSolved: 0, totalAttempts: 0 });
  const [recentPuzzles, setRecentPuzzles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!user?.id) return;

    const fetchProfileData = async () => {
      try {
        setLoading(true);
        setError(null);

        // Fetch profile stats
        const statsResponse = await api.get(`/game/profile/${user.id}`);
        setStats(statsResponse.data);

        // Fetch recent puzzles
        const recentResponse = await api.get(`/game/profile/${user.id}/recent?limit=10`);
        setRecentPuzzles(recentResponse.data);
      } catch (err) {
        setError("Failed to load profile data");
        console.error("Profile fetch error:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchProfileData();
  }, [user?.id]);

  if (loading) {
    return (
      <div style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
        color: "#f2ecff",
        fontSize: "18px"
      }}>
        Loading your profile...
      </div>
    );
  }

  if (error) {
    return (
      <div style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
        color: "#f2ecff",
        textAlign: "center",
        padding: "20px"
      }}>
        <h2 style={{ color: "#f87171", marginBottom: "16px" }}>Oops!</h2>
        <p style={{ marginBottom: "20px" }}>{error}</p>
      </div>
    );
  }

  const accuracy = stats.totalAttempts > 0 ? ((stats.puzzlesSolved / stats.totalAttempts) * 100).toFixed(1) : 0;

  return (
    <div style={{
      width: "min(1120px, calc(100% - 24px))",
      margin: "0 auto 32px",
      padding: "8px"
    }}>
      {/* Header */}
      <div style={{
        marginBottom: "32px",
        padding: "32px",
        borderRadius: "24px",
        background: "rgba(255, 255, 255, 0.06)",
        border: "1px solid rgba(255, 255, 255, 0.12)",
        boxShadow: "0 20px 50px rgba(8, 11, 28, 0.16)"
      }}>
        <div style={{ display: "flex", alignItems: "center", gap: "16px", marginBottom: "16px" }}>
          <span style={{ fontSize: "48px" }}>👤</span>
          <div>
            <h1 style={{
              margin: 0,
              fontSize: "32px",
              fontWeight: "700",
              color: "#f2ecff"
            }}>Your Profile</h1>
            <p style={{
              margin: "8px 0 0 0",
              fontSize: "14px",
              color: "#cbbfe4"
            }}>{user?.email}</p>
          </div>
        </div>
      </div>

      {/* Stats Grid */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(250px, 1fr))",
        gap: "20px",
        marginBottom: "32px"
      }}>
        {/* Total Score */}
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
          }}>{stats.totalScore}</p>
        </div>

        {/* Puzzles Solved */}
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
            color: "#d8b4fe"
          }}>{stats.puzzlesSolved}</p>
        </div>

        {/* Total Attempts */}
        <div style={{
          padding: "24px",
          borderRadius: "16px",
          background: "linear-gradient(135deg, rgba(236, 72, 153, 0.15) 0%, rgba(251, 146, 60, 0.1) 100%)",
          border: "1px solid rgba(236, 72, 153, 0.3)",
          textAlign: "center"
        }}>
          <p style={{
            margin: "0 0 12px 0",
            fontSize: "12px",
            fontWeight: "600",
            color: "#cbbfe4",
            textTransform: "uppercase",
            letterSpacing: "1px"
          }}>Total Attempts</p>
          <p style={{
            margin: 0,
            fontSize: "48px",
            fontWeight: "700",
            color: "#f472b6"
          }}>{stats.totalAttempts}</p>
        </div>

        {/* Accuracy */}
        <div style={{
          padding: "24px",
          borderRadius: "16px",
          background: "linear-gradient(135deg, rgba(34, 197, 94, 0.15) 0%, rgba(59, 130, 246, 0.1) 100%)",
          border: "1px solid rgba(34, 197, 94, 0.3)",
          textAlign: "center"
        }}>
          <p style={{
            margin: "0 0 12px 0",
            fontSize: "12px",
            fontWeight: "600",
            color: "#cbbfe4",
            textTransform: "uppercase",
            letterSpacing: "1px"
          }}>Accuracy</p>
          <p style={{
            margin: 0,
            fontSize: "48px",
            fontWeight: "700",
            color: "#4ade80"
          }}>{accuracy}%</p>
        </div>
      </div>

      {/* Recent Puzzles */}
      <div style={{
        padding: "24px",
        borderRadius: "16px",
        background: "rgba(255, 255, 255, 0.06)",
        border: "1px solid rgba(255, 255, 255, 0.12)",
        boxShadow: "0 20px 50px rgba(8, 11, 28, 0.16)"
      }}>
        <h2 style={{
          margin: "0 0 24px 0",
          fontSize: "20px",
          fontWeight: "700",
          color: "#f2ecff"
        }}>🕒 Recent Puzzles</h2>

        {recentPuzzles.length === 0 ? (
          <p style={{ color: "#cbbfe4", textAlign: "center", padding: "40px" }}>
            No puzzles solved yet. Start playing to see your progress here!
          </p>
        ) : (
          <div style={{ display: "grid", gap: "12px" }}>
            {recentPuzzles.map((puzzle, index) => (
              <div key={puzzle.puzzleId} style={{
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
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#f2ecff"
                  }}>
                    {puzzle.answer}
                  </p>
                  <p style={{
                    margin: 0,
                    fontSize: "12px",
                    color: "#cbbfe4"
                  }}>
                    {puzzle.packName} • {puzzle.attempts} attempt{puzzle.attempts !== 1 ? 's' : ''} • {puzzle.score} points
                  </p>
                </div>
                <div style={{ textAlign: "right" }}>
                  <p style={{
                    margin: 0,
                    fontSize: "12px",
                    color: "#a8e6cf",
                    fontWeight: "600"
                  }}>
                    {new Date(puzzle.solvedAt).toLocaleDateString()}
                  </p>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}