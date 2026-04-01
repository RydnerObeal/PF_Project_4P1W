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
        <div style={{ padding: 24, maxWidth: 500, margin: "0 auto" }}>
            <h1>🏷️ Manage Tags</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}

            <form onSubmit={create} style={{ display: "flex", gap: 10, marginBottom: 24 }}>
                <input value={name} onChange={e => setName(e.target.value)}
                    placeholder="Tag name (e.g. animal, food)"
                    required
                    style={{ flex: 1, padding: 8, borderRadius: 4, border: "1px solid #ccc" }} />
                <button type="submit"
                    style={{ padding: "8px 16px", background: "#007bff", color: "#fff", border: "none", borderRadius: 4, cursor: "pointer" }}>
                    Add Tag
                </button>
            </form>

            {tags.length === 0
                ? <p style={{ color: "#666" }}>No tags yet.</p>
                : <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
                    {tags.map(t => (
                        <li key={t.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "10px 12px", borderBottom: "1px solid #eee" }}>
                            <span style={{ background: "#e9ecef", borderRadius: 4, padding: "4px 12px", fontSize: 14 }}>{t.name}</span>
                            <button onClick={() => del(t.id)}
                                style={{ background: "#dc3545", color: "#fff", border: "none", padding: "4px 12px", borderRadius: 4, cursor: "pointer" }}>
                                Delete
                            </button>
                        </li>
                    ))}
                </ul>
            }
        </div>
    );
}