import { useState, useEffect } from "react";
import { api } from "../services/api";

export default function AdminPacks() {
    const [packs, setPacks] = useState([]);
    const [puzzles, setPuzzles] = useState([]);
    const [showCreateForm, setShowCreateForm] = useState(false);
    const [editingPack, setEditingPack] = useState(null);
    const [formData, setFormData] = useState({
        name: "",
        description: "",
        visibility: false
    });
    const [selectedPack, setSelectedPack] = useState(null);
    const [availablePuzzles, setAvailablePuzzles] = useState([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        loadPacks();
        loadPuzzles();
    }, []);

    const loadPacks = async () => {
        try {
            const response = await api.get("/cms/packs");
            setPacks(response.data);
        } catch (error) {
            console.error("Failed to load packs:", error);
        }
    };

    const loadPuzzles = async () => {
        try {
            const response = await api.get("/cms/puzzles");
            setPuzzles(response.data);
        } catch (error) {
            console.error("Failed to load puzzles:", error);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            if (editingPack) {
                await api.put(`/cms/packs/${editingPack.id}`, formData);
            } else {
                await api.post("/cms/packs", formData);
            }
            await loadPacks();
            resetForm();
        } catch (error) {
            console.error("Failed to save pack:", error);
            alert("Failed to save pack");
        } finally {
            setLoading(false);
        }
    };

    const handleEdit = (pack) => {
        setEditingPack(pack);
        setFormData({
            name: pack.name,
            description: pack.description || "",
            visibility: pack.status === "Published"
        });
        setShowCreateForm(true);
    };

    const handleDelete = async (pack) => {
        if (!confirm(`Delete pack "${pack.name}"? This will not delete the puzzles.`)) return;

        try {
            await api.delete(`/cms/packs/${pack.id}`);
            await loadPacks();
        } catch (error) {
            console.error("Failed to delete pack:", error);
            alert("Failed to delete pack");
        }
    };

    const handleTogglePublish = async (pack) => {
        const originalStatus = pack.status;
        const updatedStatus = originalStatus === "Published" ? "Draft" : "Published";

        setPacks(prev => prev.map(p => p.id === pack.id ? { ...p, status: updatedStatus } : p));

        try {
            await api.post(`/cms/packs/${pack.id}/publish`);
        } catch (error) {
            setPacks(prev => prev.map(p => p.id === pack.id ? { ...p, status: originalStatus } : p));
            console.error("Failed to toggle publish:", error);
            alert("Failed to toggle publish status");
        }
    };

    const handleManagePuzzles = async (pack) => {
        setSelectedPack(pack);
        // Ensure puzzles are loaded before opening modal
        if (puzzles.length === 0) {
            await loadPuzzles();
        }
    };

    const handleAddPuzzle = async (puzzleId) => {
        if (!selectedPack) return;

        try {
            await api.post(`/cms/packs/${selectedPack.id}/puzzles`, { puzzleId });
            // Reload puzzles to update the modal immediately
            await loadPuzzles();
            // Refresh the selected pack to show updated puzzle list
            setSelectedPack(prev => prev ? { ...prev } : null);
        } catch (error) {
            console.error("Failed to add puzzle:", error);
            alert("Failed to add puzzle to pack");
        }
    };

    const handleRemovePuzzle = async (puzzleId) => {
        if (!selectedPack) return;

        try {
            await api.delete(`/cms/packs/${selectedPack.id}/puzzles/${puzzleId}`);
            // Reload puzzles to update the modal immediately
            await loadPuzzles();
            // Refresh the selected pack to show updated puzzle list
            setSelectedPack(prev => prev ? { ...prev } : null);
        } catch (error) {
            console.error("Failed to remove puzzle:", error);
            alert("Failed to remove puzzle from pack");
        }
    };

    const resetForm = () => {
        setFormData({
            name: "",
            description: "",
            visibility: false
        });
        setEditingPack(null);
        setShowCreateForm(false);
    };

    const getPackPuzzles = (pack) => {
        // Filter puzzles that belong to this specific pack
        if (!pack || !pack.id) return [];
        
        const packPuzzles = puzzles.filter(p => {
            if (!p.packMemberships || !Array.isArray(p.packMemberships)) {
                return false;
            }
            return p.packMemberships.some(pm => pm.packId === pack.id);
        });
        
        console.log(`Pack: ${pack.name}, Found ${packPuzzles.length} puzzles`);
        return packPuzzles;
    };

    const getAvailablePuzzles = (pack) => {
        // Filter puzzles that DON'T belong to this pack
        if (!pack || !pack.id) return puzzles;
        
        const packPuzzleIds = getPackPuzzles(pack).map(p => p.id);
        const available = puzzles.filter(p => !packPuzzleIds.includes(p.id));
        
        console.log(`Pack: ${pack.name}, Available puzzles: ${available.length}`);
        return available;
    };

    return (
        <div className="page-shell admin-page">
            <div className="page-header">
                <div>
                    <h1>📦 Pack Playground</h1>
                    <p className="text-muted">Build and publish puzzle packs for your players.</p>
                </div>
                <button
                    onClick={() => setShowCreateForm(true)}
                    className="btn btn--primary"
                >
                    New Pack
                </button>
            </div>

            {showCreateForm && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
                    <div className="bg-white rounded-lg p-6 max-w-md w-full">
                        <h2 className="text-2xl font-bold mb-4">
                            {editingPack ? "Edit Pack" : "Create New Pack"}
                        </h2>

                        <form onSubmit={handleSubmit} className="space-y-4">
                            <div>
                                <label className="block text-sm font-medium mb-1">Name *</label>
                                <input
                                    type="text"
                                    value={formData.name}
                                    onChange={(e) => setFormData(prev => ({ ...prev, name: e.target.value }))}
                                    className="w-full p-2 border rounded"
                                    required
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium mb-1">Description</label>
                                <textarea
                                    value={formData.description}
                                    onChange={(e) => setFormData(prev => ({ ...prev, description: e.target.value }))}
                                    className="w-full p-2 border rounded"
                                    rows="3"
                                />
                            </div>

                            <div className="flex items-center">
                                <input
                                    type="checkbox"
                                    id="visibility"
                                    checked={formData.visibility}
                                    onChange={(e) => setFormData(prev => ({ ...prev, visibility: e.target.checked }))}
                                    className="mr-2"
                                />
                                <label htmlFor="visibility" className="text-sm font-medium">Published</label>
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
                                    disabled={loading}
                                    className="btn btn--primary"
                                >
                                    {loading ? "Saving..." : "Save"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {selectedPack && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
                    <div className="bg-white rounded-lg p-6 max-w-4xl w-full max-h-[90vh] overflow-y-auto">
                        <div className="flex justify-between items-center mb-6">
                            <div>
                                <h2 className="text-3xl font-bold">Manage Puzzles</h2>
                                <p className="text-gray-600 mt-1">📦 {selectedPack.name}</p>
                            </div>
                            <button
                                onClick={() => setSelectedPack(null)}
                                className="btn btn--secondary btn--small"
                            >
                                ✕
                            </button>
                        </div>

                        <div className="grid grid-cols-2 gap-6">
                            <div>
                                <h3 className="text-lg font-semibold mb-2">Available Puzzles</h3>
                                <div className="space-y-2 max-h-96 overflow-y-auto">
                                    {getAvailablePuzzles(selectedPack).map(puzzle => (
                                        <div key={puzzle.id} className="border rounded p-2 flex justify-between items-center">
                                            <div>
                                                <div className="font-medium">{puzzle.answerWord}</div>
                                                <div className="flex">
                                                    {puzzle.imageUrls.slice(0, 4).map((url, index) => (
                                                        <img
                                                            key={index}
                                                            src={url?.startsWith("http") ? url : `http://localhost:5208${url}`}
                                                            alt={`Image ${index + 1}`}
                                                            className="w-8 h-8 object-cover rounded mr-1"
                                                        />
                                                    ))}
                                                </div>
                                            </div>
                                            <button
                                                onClick={() => handleAddPuzzle(puzzle.id)}
                                                className="btn btn--primary btn--small"
                                            >
                                                Add
                                            </button>
                                        </div>
                                    ))}
                                </div>
                            </div>

                            <div>
                                <h3 className="text-lg font-semibold mb-2">Pack Puzzles ({getPackPuzzles(selectedPack).length})</h3>
                                <div className="space-y-2 max-h-96 overflow-y-auto">
                                    {getPackPuzzles(selectedPack).map(puzzle => (
                                        <div key={puzzle.id} className="border rounded p-2 flex justify-between items-center">
                                            <div>
                                                <div className="font-medium">{puzzle.answerWord}</div>
                                                <div className="flex">
                                                    {puzzle.imageUrls.slice(0, 4).map((url, index) => (
                                                        <img
                                                            key={index}
                                                            src={url?.startsWith("http") ? url : `http://localhost:5208${url}`}
                                                            alt={`Image ${index + 1}`}
                                                            className="w-8 h-8 object-cover rounded mr-1"
                                                        />
                                                    ))}
                                                </div>
                                            </div>
                                            <button
                                                onClick={() => handleRemovePuzzle(puzzle.id)}
                                                className="btn btn--danger btn--small"
                                            >
                                                Remove
                                            </button>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            <div className="space-y-4">
                {packs.map(pack => (
                    <div key={pack.id} className="border rounded p-4 bg-white shadow">
                        <div className="flex justify-between items-center">
                            <div>
                                <h3 className="text-xl font-semibold">{pack.name}</h3>
                                <p className="text-gray-600">{pack.description}</p>
                            </div>
                            <div className="flex space-x-2">
                                <button
                                    onClick={() => handleTogglePublish(pack)}
                                    className={"btn btn--primary btn--small"}
                                >
                                    {pack.status === "Published" ? "Unpublish" : "Publish"}
                                </button>
                                <button
                                    onClick={() => handleEdit(pack)}
                                    className="btn btn--secondary btn--small"
                                >
                                    Edit
                                </button>
                                <button
                                    onClick={() => handleDelete(pack)}
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