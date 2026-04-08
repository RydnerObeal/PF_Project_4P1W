import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import api from "../services/api";

export default function PacksPage() {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [packs, setPacks] = useState([]);
  const [packProgress, setPackProgress] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [refreshing, setRefreshing] = useState(false);

  const fetchPackProgress = async (packsData) => {
    if (user?.id && packsData.length > 0) {
      const progressData = {};
      for (const pack of packsData) {
        try {
          const packResponse = await api.get(`/game/profile/${user.id}/pack/${pack.id}`);
          console.log(`Pack ${pack.name} stats:`, packResponse.data);
          progressData[pack.id] = packResponse.data || { packScore: 0, packPuzzlesSolved: 0 };
        } catch (err) {
          console.error(`Error fetching progress for pack ${pack.name}:`, err);
          progressData[pack.id] = { packScore: 0, packPuzzlesSolved: 0 };
        }
      }
      setPackProgress(progressData);
    }
  };

  useEffect(() => {
    const fetchPacks = async () => {
      try {
        setLoading(true);
        const response = await api.get("/packs?random=true");
        setPacks(response.data);
        
        // Fetch progress for each pack
        await fetchPackProgress(response.data);
      } catch (err) {
        const message = err.response?.data?.message || err.message || "Failed to load packs. Please try again later.";
        setError(message);
        console.error("Error fetching packs:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchPacks();
  }, [user?.id]);

  const handleRefreshStats = async () => {
    setRefreshing(true);
    await fetchPackProgress(packs);
    setRefreshing(false);
  };

  if (loading) {
    return (
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '50vh',
        fontSize: '16px',
        color: '#cbbfe4'
      }}>
        Loading puzzle packs...
      </div>
    );
  }

  if (error) {
    return (
      <div style={{
        textAlign: 'center',
        padding: '40px',
        color: '#ff6b9d',
        fontSize: '16px'
      }}>
        {error}
      </div>
    );
  }

  if (packs.length === 0) {
    return (
      <div style={{
        textAlign: 'center',
        padding: '40px',
        color: '#cbbfe4',
        fontSize: '16px'
      }}>
        No packs available. Check back later!
      </div>
    );
  }

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
        <div style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "flex-start",
          gap: "20px"
        }}>
          <div>
            <h1 style={{
              margin: "0 0 8px 0",
              fontSize: "32px",
              fontWeight: "700",
              color: "#f2ecff"
            }}>🎮 Choose a Pack</h1>
            <p style={{
              margin: 0,
              fontSize: "14px",
              color: "#cbbfe4"
            }}>Complete puzzles to earn points and track your progress</p>
          </div>
          <button
            onClick={handleRefreshStats}
            disabled={refreshing}
            style={{
              padding: "10px 16px",
              borderRadius: "12px",
              background: "rgba(200, 160, 255, 0.2)",
              border: "1px solid rgba(200, 160, 255, 0.4)",
              color: "#d8b4fe",
              fontSize: "13px",
              fontWeight: "600",
              cursor: refreshing ? "not-allowed" : "pointer",
              transition: "all 0.2s ease",
              opacity: refreshing ? 0.6 : 1
            }}
            onMouseEnter={(e) => {
              if (!refreshing) {
                e.target.style.background = "rgba(200, 160, 255, 0.3)";
              }
            }}
            onMouseLeave={(e) => {
              e.target.style.background = "rgba(200, 160, 255, 0.2)";
            }}
          >
            {refreshing ? "Refreshing..." : "🔄 Refresh Scores"}
          </button>
        </div>
      </div>

      {/* Packs Grid */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fill, minmax(300px, 1fr))",
        gap: "20px"
      }}>
        {packs.map((pack) => {
          const progress = packProgress[pack.id] || { packScore: 0, packPuzzlesSolved: 0 };
          return (
            <div
              key={pack.id}
              style={{
                padding: "24px",
                borderRadius: "16px",
                background: "rgba(255, 255, 255, 0.06)",
                border: "1px solid rgba(255, 255, 255, 0.12)",
                transition: "all 0.3s ease",
                cursor: "pointer"
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.background = "rgba(255, 255, 255, 0.08)";
                e.currentTarget.style.borderColor = "rgba(194, 136, 255, 0.4)";
                e.currentTarget.style.transform = "translateY(-4px)";
                e.currentTarget.style.boxShadow = "0 24px 60px rgba(139, 92, 246, 0.2)";
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.background = "rgba(255, 255, 255, 0.06)";
                e.currentTarget.style.borderColor = "rgba(255, 255, 255, 0.12)";
                e.currentTarget.style.transform = "translateY(0)";
                e.currentTarget.style.boxShadow = "0 20px 50px rgba(8, 11, 28, 0.16)";
              }}
            >
              {/* Pack Title */}
              <h2 style={{
                margin: "0 0 12px 0",
                fontSize: "20px",
                fontWeight: "700",
                color: "#f2ecff"
              }}>{pack.name}</h2>

              {/* Description */}
              <p style={{
                margin: "0 0 16px 0",
                fontSize: "13px",
                color: "#cbbfe4",
                lineHeight: "1.5"
              }}>{pack.description}</p>

              {/* Stats */}
              <div style={{
                display: "grid",
                gridTemplateColumns: "1fr 1fr",
                gap: "12px",
                marginBottom: "16px",
                padding: "12px",
                borderRadius: "12px",
                background: "rgba(255, 255, 255, 0.04)",
                border: "1px solid rgba(255, 255, 255, 0.08)"
              }}>
                <div style={{ textAlign: "center" }}>
                  <p style={{
                    margin: "0 0 4px 0",
                    fontSize: "11px",
                    color: "#cbbfe4",
                    textTransform: "uppercase",
                    fontWeight: "600"
                  }}>Puzzles</p>
                  <p style={{
                    margin: 0,
                    fontSize: "18px",
                    fontWeight: "700",
                    color: "#d8b4fe"
                  }}>{pack.puzzleCount}</p>
                </div>
                <div style={{ textAlign: "center" }}>
                  <p style={{
                    margin: "0 0 4px 0",
                    fontSize: "11px",
                    color: "#cbbfe4",
                    textTransform: "uppercase",
                    fontWeight: "600"
                  }}>High Score</p>
                  <p style={{
                    margin: 0,
                    fontSize: "18px",
                    fontWeight: "700",
                    color: "#f472b6"
                  }}>{progress.packScore}</p>
                </div>
              </div>

              {/* Solved Counter */}
              {progress.packPuzzlesSolved > 0 && (
                <p style={{
                  margin: "0 0 16px 0",
                  fontSize: "12px",
                  color: "#a8e6cf",
                  fontWeight: "600"
                }}>
                  ✓ {progress.packPuzzlesSolved} of {pack.puzzleCount} solved
                </p>
              )}

              {/* Progress Bar */}
              <div style={{
                marginBottom: "16px",
                height: "8px",
                borderRadius: "4px",
                background: "rgba(255, 255, 255, 0.1)",
                overflow: "hidden"
              }}>
                <div style={{
                  height: "100%",
                  width: `${(progress.packPuzzlesSolved / pack.puzzleCount) * 100}%`,
                  background: "linear-gradient(90deg, #8b5cf6 0%, #c084fc 100%)",
                  transition: "width 0.3s ease"
                }}></div>
              </div>

              {/* Play Button */}
              <button
                onClick={() => navigate(`/play/${pack.id}`)}
                style={{
                  width: "100%",
                  padding: "12px 16px",
                  borderRadius: "12px",
                  background: "linear-gradient(135deg, #8b5cf6 0%, #c084fc 100%)",
                  color: "#ffffff",
                  border: "1px solid rgba(255, 255, 255, 0.2)",
                  fontSize: "13px",
                  fontWeight: "700",
                  cursor: "pointer",
                  transition: "all 0.2s ease",
                  boxShadow: "0 8px 20px rgba(139, 92, 246, 0.2)"
                }}
                onMouseEnter={(e) => {
                  e.target.style.transform = "translateY(-2px)";
                  e.target.style.boxShadow = "0 12px 28px rgba(139, 92, 246, 0.3)";
                }}
                onMouseLeave={(e) => {
                  e.target.style.transform = "translateY(0)";
                  e.target.style.boxShadow = "0 8px 20px rgba(139, 92, 246, 0.2)";
                }}
              >
                Play
              </button>
            </div>
          );
        })}
      </div>
    </div>
  );
}
