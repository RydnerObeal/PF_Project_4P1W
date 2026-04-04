import { useState, useEffect } from "react";
import api from "../services/api";

export default function AdminTags() {
    const [tags, setTags] = useState([]);
    const [name, setName] = useState("");
    const [error, setError] = useState(null);

    const load = () => api.get("/cms/tags").then(r => setTags(r.data)).catch(() => setError("Failed to load tags."));

    useEffect(() => { load(); }, []);

    const create = async (e) => {
        e.preventDefault();
        try {
            await api.post("/cms/tags", { name });
            setName("");
            setError(null);
            load();
        } catch (err) {
            setError(err.response?.data || "Failed to create tag.");
        }
    };

    const del = async (id) => {
        if (!confirm("Delete this tag?")) return;
        await api.delete(`/cms/tags/${id}`);
        load();
    };

    return (
        <div className="page-shell admin-page">
            <div className="page-header">
                <div>
                    <h1>🏷️ Tag Garden</h1>
                    <p className="text-muted">Keep your game tags bright and easy to use.</p>
                </div>
                <button type="submit" form="tag-form" className="btn btn--primary">
                    Add Tag
                </button>
            </div>
            {error && <p style={{ color: "#f87171" }}>{error}</p>}

            <form id="tag-form" onSubmit={create} className="form-row">
                <input value={name} onChange={e => setName(e.target.value)}
                    placeholder="Tag name (e.g. animal, food)"
                    required
                    className="input-field" />
            </form>

            {tags.length === 0
                ? <p style={{ color: "#666" }}>No tags yet.</p>
                : <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
                    {tags.map(t => (
                        <li key={t.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "10px 12px", borderBottom: "1px solid #eee" }}>
                            <span className="tag-badge">{t.name}</span>
                            <button onClick={() => del(t.id)} className="btn btn--danger btn--small">
                                Delete
                            </button>
                        </li>
                    ))}
                </ul>
            }
        </div>
    );
}