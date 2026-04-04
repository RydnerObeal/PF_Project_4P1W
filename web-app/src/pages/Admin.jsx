import { useNavigate } from "react-router-dom";

export default function Admin() {
    const navigate = useNavigate();
    return (
        <div style={{ padding: 24, maxWidth: 500, margin: "0 auto" }}>
            <h1>Admin CMS</h1>
            <p>Manage your 4 Pics 1 Word content below.</p>
            <div style={{ display: "flex", flexDirection: "column", gap: 12, marginTop: 24 }}>
                <button onClick={() => navigate("/admin/images")}
                    style={{ padding: "12px 20px", background: "#007bff", color: "#fff", border: "none", borderRadius: 6, cursor: "pointer", fontSize: 16, textAlign: "left" }}>
                    📷 Manage Images
                </button>
                <button onClick={() => navigate("/admin/tags")}
                    style={{ padding: "12px 20px", background: "#6c757d", color: "#fff", border: "none", borderRadius: 6, cursor: "pointer", fontSize: 16, textAlign: "left" }}>
                    🏷️ Manage Tags
                </button>
                <button onClick={() => navigate("/admin/puzzles")}
                    style={{ padding: "12px 20px", background: "#28a745", color: "#fff", border: "none", borderRadius: 6, cursor: "pointer", fontSize: 16, textAlign: "left" }}>
                    🧩 Manage Puzzles
                </button>
                <button onClick={() => navigate("/admin/packs")}
                    style={{ padding: "12px 20px", background: "#ffc107", color: "#000", border: "none", borderRadius: 6, cursor: "pointer", fontSize: 16, textAlign: "left" }}>
                    📦 Manage Packs
                </button>
            </div>
        </div>
    );
}