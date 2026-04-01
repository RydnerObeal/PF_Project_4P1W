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
        <div style={{ padding: 24, maxWidth: 1000, margin: "0 auto" }}>
            <h1>📷 Manage Images</h1>

            {error && <p style={{ color: "red" }}>{error}</p>}

            {/* Add Image Form */}
            <form onSubmit={addImage} style={{ display: "flex", gap: 10, marginBottom: 24, flexWrap: "wrap" }}>
                <input value={url} onChange={e => setUrl(e.target.value)}
                    placeholder="Image URL (required)" required
                    style={{ flex: 2, minWidth: 200, padding: 8, borderRadius: 4, border: "1px solid #ccc" }} />
                <input value={fileName} onChange={e => setFileName(e.target.value)}
                    placeholder="File name (optional)"
                    style={{ flex: 1, minWidth: 140, padding: 8, borderRadius: 4, border: "1px solid #ccc" }} />
                <button type="submit"
                    style={{ padding: "8px 16px", background: "#007bff", color: "#fff", border: "none", borderRadius: 4, cursor: "pointer" }}>
                    Add Image
                </button>
            </form>

            {/* Filter by Tag */}
            <div style={{ marginBottom: 16 }}>
                <label style={{ marginRight: 8 }}>Filter by tag:</label>
                <select value={filterTag} onChange={e => setFilterTag(e.target.value)}
                    style={{ padding: "6px 10px", borderRadius: 4 }}>
                    <option value="">All</option>
                    {tags.map(t => <option key={t.id} value={t.name}>{t.name}</option>)}
                </select>
            </div>

            {/* Image Grid */}
            {images.length === 0
                ? <p style={{ color: "#666" }}>No images found.</p>
                : <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(200px, 1fr))", gap: 16 }}>
                    {images.map(img => (
                        <div key={img.id} style={{ border: "1px solid #ddd", borderRadius: 8, overflow: "hidden", background: "#fff" }}>
                            <img src={img.url} alt={img.fileName}
                                onError={e => { e.target.src = "https://placehold.co/200x150?text=No+Image"; }}
                                style={{ width: "100%", height: 140, objectFit: "cover" }} />
                            <div style={{ padding: 10 }}>
                                <p style={{ fontSize: 12, color: "#666", margin: "0 0 8px", wordBreak: "break-all" }}>
                                    {img.fileName || "No name"}
                                </p>

                                {/* Tags */}
                                <div style={{ display: "flex", flexWrap: "wrap", gap: 4, marginBottom: 8 }}>
                                    {img.tags?.map(tag => (
                                        <span key={tag} style={{ background: "#e9ecef", borderRadius: 4, padding: "2px 8px", fontSize: 12, display: "flex", alignItems: "center", gap: 4 }}>
                                            {tag}
                                            <button onClick={() => removeTag(img.id, tag)}
                                                style={{ border: "none", background: "none", color: "red", cursor: "pointer", padding: 0, fontSize: 14, lineHeight: 1 }}>×</button>
                                        </span>
                                    ))}
                                </div>

                                {/* Add Tag */}
                                <select defaultValue="" onChange={e => { addTag(img.id, e.target.value); e.target.value = ""; }}
                                    style={{ width: "100%", padding: "4px 6px", fontSize: 12, marginBottom: 8, borderRadius: 4, border: "1px solid #ccc" }}>
                                    <option value="">+ Add tag</option>
                                    {tags.filter(t => !img.tags?.includes(t.name)).map(t => (
                                        <option key={t.id} value={t.name}>{t.name}</option>
                                    ))}
                                </select>

                                <button onClick={() => deleteImage(img.id)}
                                    style={{ width: "100%", padding: "6px", background: "#dc3545", color: "#fff", border: "none", borderRadius: 4, cursor: "pointer", fontSize: 13 }}>
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