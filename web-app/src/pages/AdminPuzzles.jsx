import { useState, useEffect } from "react";
import { api } from "../services/api";

export default function AdminPuzzles() {
    const [puzzles, setPuzzles] = useState([]);
    const [images, setImages] = useState([]);
    const [tags, setTags] = useState([]);
    const [selectedTag, setSelectedTag] = useState("");
    const [showCreateForm, setShowCreateForm] = useState(false);
    const [editingPuzzle, setEditingPuzzle] = useState(null);
    const [formData, setFormData] = useState({
        answerWord: "",
        hint: "",
        imageIds: []
    });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        loadPuzzles();
        loadTags();
    }, []);

    useEffect(() => {
        loadImages(selectedTag);
    }, [selectedTag]);

    const loadPuzzles = async () => {
        try {
            const response = await api.get("/cms/puzzles");
            setPuzzles(response.data);
        } catch (error) {
            console.error("Failed to load puzzles:", error);
        }
    };

    const loadImages = async (tag = "") => {
        try {
            const query = tag ? `?tag=${encodeURIComponent(tag)}` : "";
            const response = await api.get(`/cms/images${query}`);
            setImages(response.data);
        } catch (error) {
            console.error("Failed to load images:", error);
        }
    };

    const loadTags = async () => {
        try {
            const response = await api.get("/cms/tags");
            setTags(response.data);
        } catch (error) {
            console.error("Failed to load tags:", error);
        }
    };

    const filteredImages = selectedTag
        ? images.filter(img => img.tags.some(t => t === selectedTag))
        : images;

    const handleImageSelect = (imageId) => {
        setFormData(prev => {
            const isSelected = prev.imageIds.includes(imageId);
            if (isSelected) {
                return { ...prev, imageIds: prev.imageIds.filter(id => id !== imageId) };
            } else if (prev.imageIds.length < 4) {
                return { ...prev, imageIds: [...prev.imageIds, imageId] };
            }
            return prev;
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (formData.imageIds.length !== 4) {
            alert("Please select exactly 4 images");
            return;
        }

        setLoading(true);
        try {
            if (editingPuzzle) {
                await api.put(`/cms/puzzles/${editingPuzzle.id}`, formData);
            } else {
                await api.post("/cms/puzzles", formData);
            }
            await loadPuzzles();
            resetForm();
        } catch (error) {
            console.error("Failed to save puzzle:", error);
            alert("Failed to save puzzle");
        } finally {
            setLoading(false);
        }
    };

    const handleEdit = (puzzle) => {
        setEditingPuzzle(puzzle);
        setFormData({
            answerWord: puzzle.answerWord,
            hint: puzzle.hint || "",
            imageIds: puzzle.imageIds
        });
        setShowCreateForm(true);
    };

    const handleDelete = async (puzzle) => {
        if (!confirm(`Delete puzzle "${puzzle.answerWord}"?`)) return;

        try {
            await api.delete(`/cms/puzzles/${puzzle.id}`);
            await loadPuzzles();
        } catch (error) {
            console.error("Failed to delete puzzle:", error);
            if (error.response?.data?.publishedPacks) {
                alert(`Cannot delete: belongs to published packs: ${error.response.data.publishedPacks.join(", ")}`);
            } else {
                alert("Failed to delete puzzle");
            }
        }
    };

    const resetForm = () => {
        setFormData({
            answerWord: "",
            hint: "",
            imageIds: []
        });
        setEditingPuzzle(null);
        setShowCreateForm(false);
    };

    return (
        <div className="page-shell admin-page">
            <div className="page-header">
                <div>
                    <h1>🧩 Puzzle Workshop</h1>
                    <p className="text-muted">Create playful puzzles and tune the challenge level for players.</p>
                </div>
                <button
                    onClick={() => setShowCreateForm(true)}
                    className="btn btn--primary"
                >
                    New Puzzle
                </button>
            </div>

            {showCreateForm && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
                    <div className="bg-white rounded-lg p-6 max-w-4xl w-full max-h-[90vh] overflow-y-auto">
                        <h2 className="text-2xl font-bold mb-4">
                            {editingPuzzle ? "Edit Puzzle" : "Create New Puzzle"}
                        </h2>

                        <form onSubmit={handleSubmit} className="space-y-4">
                            <div>
                                <label className="block text-sm font-medium mb-1">Answer Word *</label>
                                <input
                                    type="text"
                                    value={formData.answerWord}
                                    onChange={(e) => setFormData(prev => ({ ...prev, answerWord: e.target.value }))}
                                    className="w-full p-2 border rounded"
                                    required
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium mb-1">Hint</label>
                                <input
                                    type="text"
                                    value={formData.hint}
                                    onChange={(e) => setFormData(prev => ({ ...prev, hint: e.target.value }))}
                                    className="w-full p-2 border rounded"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium mb-2">
                                    Images ({formData.imageIds.length}/4 selected)
                                </label>

                                <div className="mb-4">
                                    <label className="block text-sm font-medium mb-1">Filter by Tag</label>
                                    <select
                                        value={selectedTag}
                                        onChange={(e) => setSelectedTag(e.target.value)}
                                        className="p-2 border rounded"
                                    >
                                        <option value="">All tags</option>
                                        {tags.map(tag => (
                                            <option key={tag.id} value={tag.name}>{tag.name}</option>
                                        ))}
                                    </select>
                                </div>

                                <div className="grid grid-cols-4 gap-4 max-h-96 overflow-y-auto">
                                    {filteredImages.map(image => (
                                        <div
                                            key={image.id}
                                            onClick={() => handleImageSelect(image.id)}
                                            className={`cursor-pointer border-2 rounded p-2 ${
                                                formData.imageIds.includes(image.id)
                                                    ? "border-blue-500 bg-blue-50"
                                                    : "border-gray-300"
                                            }`}
                                        >
                                            <img
                                                src={image.url?.startsWith("http") ? image.url : `http://localhost:5208${image.url}`}
                                                alt="Puzzle image"
                                                className="w-full h-20 object-cover rounded"
                                            />
                                            <div className="text-xs mt-1">
                                                {image.tags.join(", ")}
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>

                            <div className="flex justify-end space-x-2">
                                <button
                                    type="button"
                                    onClick={resetForm}
                                    className="btn btn--secondary"
                                >
                                    Cancel
                                </button>
                                <button
                                    type="submit"
                                    disabled={loading || formData.imageIds.length !== 4}
                                    className="btn btn--primary"
                                >
                                    {loading ? "Saving..." : "Save"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            <div className="grid gap-4">
                {puzzles.map(puzzle => (
                    <div key={puzzle.id} className="border rounded p-4 bg-white shadow">
                        <div className="flex justify-between items-start">
                            <div className="flex-1">
                                <h3 className="text-xl font-semibold">{puzzle.answerWord}</h3>
                                <p className="text-gray-600">{puzzle.hint}</p>
                                <div className="flex mt-2">
                                    {puzzle.imageUrls.map((url, index) => (
                                        <img
                                            key={index}
                                            src={url?.startsWith("http") ? url : `http://localhost:5208${url}`}
                                            alt={`Image ${index + 1}`}
                                            className="w-16 h-16 object-cover rounded mr-2"
                                        />
                                    ))}
                                </div>
                                <div className="mt-2">
                                    <span className="text-sm text-gray-500">Packs: </span>
                                    {puzzle.packMemberships.map(pm => (
                                        <span
                                            key={pm.packId}
                                            className={`inline-block px-2 py-1 rounded text-xs mr-1 ${
                                                pm.isPublished ? "bg-green-100 text-green-800" : "bg-gray-100 text-gray-800"
                                            }`}
                                        >
                                            {pm.packName}
                                        </span>
                                    ))}
                                </div>
                            </div>
                            <div className="flex space-x-2">
                                <button
                                    onClick={() => handleEdit(puzzle)}
                                    className="btn btn--secondary btn--small"
                                >
                                    Edit
                                </button>
                                <button
                                    onClick={() => handleDelete(puzzle)}
                                    className="btn btn--danger btn--small"
                                >
                                    Delete
                                </button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}