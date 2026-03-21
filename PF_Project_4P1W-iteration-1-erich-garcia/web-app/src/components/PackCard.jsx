import { useNavigate } from "react-router-dom";

export default function PackCard({ pack }) {
  const navigate = useNavigate();

  const handlePlayClick = () => {
    navigate(`/play/${pack.id}`);
  };

  return (
    <div style={{
      border: '1px solid #ddd',
      borderRadius: '8px',
      padding: '20px',
      margin: '10px',
      backgroundColor: '#fff',
      boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
      maxWidth: '300px'
    }}>
      <h3 style={{ margin: '0 0 10px 0', color: '#333' }}>
        {pack.name}
      </h3>
      <p style={{ 
        margin: '0 0 15px 0', 
        color: '#666',
        fontSize: '14px',
        lineHeight: '1.4'
      }}>
        {pack.description}
      </p>
      <button
        onClick={handlePlayClick}
        style={{
          backgroundColor: '#007bff',
          color: '#fff',
          border: 'none',
          borderRadius: '4px',
          padding: '8px 16px',
          cursor: 'pointer',
          fontSize: '14px',
          fontWeight: 'bold'
        }}
        onMouseOver={(e) => e.target.style.backgroundColor = '#0056b3'}
        onMouseOut={(e) => e.target.style.backgroundColor = '#007bff'}
      >
        Play
      </button>
    </div>
  );
}
