import { BrowserRouter, Routes, Route, Link, useNavigate } from "react-router-dom";
import "./App.css";
import { AuthProvider, useAuth } from "./context/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminRoute from "./components/AdminRoute";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Home from "./pages/Home";
import PacksPage from "./pages/PacksPage";
import PlayPage from "./pages/PlayPage";
import AdminImages from "./pages/AdminImages";
import AdminTags from "./pages/AdminTags";
import AdminPuzzles from "./pages/AdminPuzzles";
import AdminPacks from "./pages/AdminPacks";

function AppLayout() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login");
    };

    return (
        <>
            {user && (
                <header className="app-header">
                    <div className="app-header__content">
                        <div>
                            <span className="app-header__user">👤 {user.email}</span>
                            {user.role === "admin" && <span className="app-header__role">Admin</span>}
                        </div>
                        <button onClick={handleLogout} className="btn btn--secondary btn--small">
                            Logout
                        </button>
                    </div>
                </header>
            )}
            {user?.role === "admin" && (
                <nav className="admin-nav">
                    <Link className="admin-nav__link" to="/admin/images">Admin Images</Link>
                    <Link className="admin-nav__link" to="/admin/tags">Admin Tags</Link>
                    <Link className="admin-nav__link" to="/admin/puzzles">Admin Puzzles</Link>
                    <Link className="admin-nav__link" to="/admin/packs">Admin Packs</Link>
                </nav>
            )}
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/" element={<ProtectedRoute><Home /></ProtectedRoute>} />
                <Route path="/packs" element={<ProtectedRoute><PacksPage /></ProtectedRoute>} />
                <Route path="/play/:packId" element={<ProtectedRoute><PlayPage /></ProtectedRoute>} />
                <Route path="/admin/images" element={<AdminRoute><AdminImages /></AdminRoute>} />
                <Route path="/admin/tags" element={<AdminRoute><AdminTags /></AdminRoute>} />
                <Route path="/admin/puzzles" element={<AdminRoute><AdminPuzzles /></AdminRoute>} />
                <Route path="/admin/packs" element={<AdminRoute><AdminPacks /></AdminRoute>} />
            </Routes>
        </>
    );
}

export default function App() {
    return (
        <BrowserRouter>
            <AuthProvider>
                <AppLayout />
            </AuthProvider>
        </BrowserRouter>
    );
}
