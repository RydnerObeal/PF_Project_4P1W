import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import PackCard from "../components/PackCard";

export default function PacksPage() {
  const navigate = useNavigate();
  const [packs, setPacks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPacks = async () => {
      try {
        setLoading(true);
        const response = await api.get("/packs?random=true");
        setPacks(response.data);
      } catch (err) {
        const message = err.response?.data?.message || err.message || "Failed to load packs. Please try again later.";
        setError(message);
        console.error("Error fetching packs:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchPacks();
  }, []);

  if (loading) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '50vh',
        fontSize: '18px',
        color: '#666'
      }}>
        Loading packs...
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ 
        textAlign: 'center', 
        padding: '40px',
        color: '#dc3545',
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
        color: '#666',
        fontSize: '16px'
      }}>
        No packs available. Check back later!
      </div>
    );
  }

  return (
    <div style={{ padding: "20px" }}>
      <h1>Packs</h1>
      <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(250px, 1fr))", gap: "20px" }}>
        {packs.map((pack) => (
          <div key={pack.id} style={{ border: "1px solid #ccc", padding: "20px", borderRadius: "8px" }}>
            <h3>{pack.name}</h3>
            <p>{pack.description}</p>
            <button style={{ padding: "10px 20px", backgroundColor: "#007bff", color: "white", border: "none", borderRadius: "4px", cursor: "pointer" }} onClick={() => navigate(`/play/${pack.id}`)}>Play</button>
          </div>
        ))}
      </div>
    </div>
  );
}
