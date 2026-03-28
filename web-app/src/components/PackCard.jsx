import { useNavigate } from "react-router-dom";

export default function PackCard({ pack }) {
  const navigate = useNavigate();

  const handlePlayClick = () => {
    navigate(`/play/${pack.id}`);
  };

  return (
    <article className="pack-card">
      <div className="pack-card__header">
        <h3>{pack.name}</h3>
      </div>
      <p>{pack.description}</p>
      <button className="btn btn--primary" onClick={handlePlayClick}>Play</button>
    </article>
  );
}
