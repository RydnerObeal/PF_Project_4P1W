import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminRoute from "./components/AdminRoute";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Home from "./pages/Home";
import PacksPage from "./pages/PacksPage";
import PlayPage from "./pages/PlayPage";
import AdminImages from "./pages/AdminImages";
import AdminTags from "./pages/AdminTags";

export default function App() {
    return (
        <BrowserRouter>
            <AuthProvider>
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/" element={<ProtectedRoute><Home /></ProtectedRoute>} />
                    <Route path="/packs" element={<ProtectedRoute><PacksPage /></ProtectedRoute>} />
                    <Route path="/play/:packId" element={<ProtectedRoute><PlayPage /></ProtectedRoute>} />
                    <Route path="/admin/images" element={<AdminRoute><AdminImages /></AdminRoute>} />
                    <Route path="/admin/tags" element={<AdminRoute><AdminTags /></AdminRoute>} />
                </Routes>
            </AuthProvider>
        </BrowserRouter>
    );
}