import { useState, useEffect } from "react";
import api from "../services/api";
import PackCard from "../components/PackCard";

export default function PacksPage() {
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
        setError("Failed to load packs. Please try again later.");
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
    <div style={{ padding: '20px' }}>
      <h1 style={{ 
        textAlign: 'center', 
        marginBottom: '30px',
        color: '#333'
      }}>
        Choose a Pack
      </h1>
      <div style={{
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'center',
        gap: '20px'
      }}>
        {packs.map((pack) => (
          <PackCard key={pack.id} pack={pack} />
        ))}
      </div>
    </div>
  );
}
