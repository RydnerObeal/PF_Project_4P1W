import { useState, useEffect } from "react";
import api from "../services/api";

export default function AdminImages() {
    const [images, setImages] = useState([]);
    const [tags, setTags] = useState([]);
    const [url, setUrl] = useState("");
    const [fileName, setFileName] = useState("");
    const [filterTag, setFilterTag] = useState("");
    const [error, setError] = useState(null);

    const load = async () => {
        try {
            const [imgRes, tagRes] = await Promise.all([
                api.get(`/cms/images${filterTag ? `?tag=${filterTag}` : ""}`),
                api.get("/cms/tags")
            ]);
            setImages(imgRes.data);
            setTags(tagRes.data);
        } catch {
            setError("Failed to load images.");
        }
    };

    useEffect(() => { load(); }, [filterTag]);

    const addImage = async (e) => {
        e.preventDefault();
        try {
            await api.post("/cms/images", { url, fileName });
            setUrl(""); setFileName("");
            load();
        } catch { setError("Failed to add image."); }
    };

    const deleteImage = async (id) => {
        if (!confirm("Delete this image?")) return;
        await api.delete(`/cms/images/${id}`);
        load();
    };

    const addTag = async (imageId, tagName) => {
        if (!tagName) return;
        await api.post(`/cms/images/${imageId}/tags`, { tagName });
        load();
    };

    const removeTag = async (imageId, tagName) => {
        await api.delete(`/cms/images/${imageId}/tags/${tagName}`);
        load();
    };

    return (
        <div className="page-shell admin-page">
            <div className="page-header">
                <div>
                    <h1>📷 Image Studio</h1>
                    <p className="text-muted">Upload images and tag them for your cute puzzle game.</p>
                </div>
            </div>

            {error && <p style={{ color: "#f87171" }}>{error}</p>}

            <form onSubmit={addImage} className="form-row">
                <input value={url} onChange={e => setUrl(e.target.value)}
                    placeholder="Image URL (required)" required
                    className="input-field" />
                <input value={fileName} onChange={e => setFileName(e.target.value)}
                    placeholder="File name (optional)"
                    className="input-field" />
                <button type="submit" className="btn btn--primary">
                    Add Image
                </button>
            </form>

            <div className="section-card" style={{ marginBottom: 16 }}>
                <div className="button-row" style={{ justifyContent: "space-between", flexWrap: "wrap", gap: "12px" }}>
                    <span className="tag-badge">Filter by tag</span>
                    <select value={filterTag} onChange={e => setFilterTag(e.target.value)}
                        className="select-field" style={{ maxWidth: 260 }}>
                        <option value="">All</option>
                        {tags.map(t => <option key={t.id} value={t.name}>{t.name}</option>)}
                    </select>
                </div>
            </div>

            {images.length === 0
                ? <p style={{ color: "var(--text-muted)" }}>No images found.</p>
                : <div className="card-grid">
                    {images.map(img => (
                        <div key={img.id} className="admin-card image-card">
                            <img src={img.url?.startsWith("http") ? img.url : `http://localhost:5208${img.url}`}
                                alt={img.fileName}
                                onError={e => { e.target.src = "https://placehold.co/400x300?text=No+Image"; }}
                                style={{ width: "100%", aspectRatio: "16/10", objectFit: "cover" }} />
                            <div style={{ padding: 16 }}>
                                <p className="tag-badge" style={{ marginBottom: 12 }}>{img.fileName || "No name"}</p>

                                <div style={{ display: "flex", flexWrap: "wrap", gap: 8, marginBottom: 12 }}>
                                    {img.tags?.map(tag => (
                                        <span key={tag} className="tag-badge">
                                            {tag}
                                            <button onClick={() => removeTag(img.id, tag)} className="btn btn--secondary btn--small" style={{ padding: '0 8px', minWidth: 'auto' }}>
                                                ×
                                            </button>
                                        </span>
                                    ))}
                                </div>

                                <select defaultValue="" onChange={e => { addTag(img.id, e.target.value); e.target.value = ""; }}
                                    className="select-field" style={{ marginBottom: 12 }}>
                                    <option value="">+ Add tag</option>
                                    {tags.filter(t => !img.tags?.includes(t.name)).map(t => (
                                        <option key={t.id} value={t.name}>{t.name}</option>
                                    ))}
                                </select>

                                <button onClick={() => deleteImage(img.id)} className="btn btn--danger btn--small" style={{ width: "100%" }}>
                                    Delete
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            }
        </div>
    );
}