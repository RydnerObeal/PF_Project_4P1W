import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useEffect, useState } from "react";
import { getTopScorer, getUserById } from "../services/api";

export default function Home() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [topScorer, setTopScorer] = useState(null);

  useEffect(() => {
    const fetchTopScorer = async () => {
      try {
        const response = await getTopScorer();
        const { userId, totalScore } = response.data;
        const userResponse = await getUserById(userId);
        const userDetails = userResponse.data;
        setTopScorer({ ...userDetails, totalScore });
      } catch (error) {
        console.error("Failed to fetch top scorer:", error);
      }
    };
    fetchTopScorer();
  }, []);

  const handlePlayClick = () => {
    navigate("/packs");
  };

  return (
    <div style={{ padding: "20px", maxWidth: "600px", margin: "0 auto" }}>
      <h2>Welcome back, {user?.email}</h2>
      <p style={{ margin: '12px 0 16px' }}>Role: <strong>{user?.role}</strong></p>

      {topScorer && (
        <div style={{ margin: '20px 0', padding: '10px', border: '1px solid #ccc', borderRadius: '4px' }}>
          <h3>Top Player</h3>
          <p><strong>Email:</strong> {topScorer.email}</p>
          <p><strong>Role:</strong> {topScorer.role}</p>
          <p><strong>Total Score:</strong> {topScorer.totalScore}</p>
        </div>
      )}

      <div style={{ display: "flex", gap: "10px", flexWrap: "wrap" }}>
        <button style={{ padding: "10px 20px", backgroundColor: "#007bff", color: "white", border: "none", borderRadius: "4px", cursor: "pointer" }} onClick={handlePlayClick}>
          Choose a Puzzle Pack
        </button>
        <button style={{ padding: "10px 20px", backgroundColor: "#6c757d", color: "white", border: "none", borderRadius: "4px", cursor: "pointer" }} onClick={logout}>
          Logout
        </button>
      </div>
    </div>
  );
}