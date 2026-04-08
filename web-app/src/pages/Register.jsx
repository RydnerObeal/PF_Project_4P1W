import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Register() {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    email: "",
    password: "",
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      await register(form.email, form.password, "player");
      alert("✅ Account created! Redirecting to login...");
      navigate("/login");
    } catch (err) {
      console.log("Full error object:", err);
      const message = 
        err.response?.data?.message || 
        err.message || 
        "Registration failed. Please try again.";
      setError(message);
      console.error("Register error:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      minHeight: "100vh",
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      padding: "20px"
    }}>
      <div style={{
        width: "100%",
        maxWidth: "420px",
        padding: "40px 32px",
        borderRadius: "24px",
        background: "rgba(255, 255, 255, 0.08)",
        border: "1px solid rgba(255, 255, 255, 0.15)",
        backdropFilter: "blur(18px)",
        boxShadow: "0 24px 72px rgba(8, 11, 28, 0.45)"
      }}>
        {/* Header */}
        <div style={{ textAlign: "center", marginBottom: "32px" }}>
          <div style={{
            fontSize: "48px",
            marginBottom: "12px"
          }}>🎮</div>
          <h1 style={{
            margin: "0 0 8px 0",
            fontSize: "32px",
            fontWeight: "700",
            color: "#f2ecff",
            letterSpacing: "-0.5px"
          }}>Join the Game</h1>
          <p style={{
            margin: 0,
            fontSize: "14px",
            color: "#cbbfe4"
          }}>Create your account and start playing</p>
        </div>

        {/* Error Message */}
        {error && (
          <div style={{
            padding: "12px 14px",
            marginBottom: "20px",
            borderRadius: "12px",
            background: "rgba(220, 53, 69, 0.15)",
            border: "1px solid rgba(220, 53, 69, 0.4)",
            color: "#ff6b9d",
            fontSize: "13px",
            fontWeight: "500"
          }}>
            {error}
          </div>
        )}

        {/* Form */}
        <form onSubmit={handleSubmit} style={{ marginBottom: "24px" }}>
          {/* Email Input */}
          <div style={{ marginBottom: "16px" }}>
            <label style={{
              display: "block",
              fontSize: "13px",
              fontWeight: "600",
              color: "#f2ecff",
              marginBottom: "8px"
            }}>Email</label>
            <input
              type="email"
              placeholder="your@email.com"
              value={form.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
              required
              disabled={loading}
              style={{
                width: "100%",
                padding: "12px 14px",
                borderRadius: "12px",
                background: "rgba(255, 255, 255, 0.08)",
                border: "1px solid rgba(255, 255, 255, 0.15)",
                color: "#f2ecff",
                fontSize: "14px",
                transition: "all 0.2s ease",
                boxSizing: "border-box",
                outline: "none"
              }}
              onFocus={(e) => {
                e.target.style.background = "rgba(255, 255, 255, 0.12)";
                e.target.style.borderColor = "rgba(194, 136, 255, 0.4)";
              }}
              onBlur={(e) => {
                e.target.style.background = "rgba(255, 255, 255, 0.08)";
                e.target.style.borderColor = "rgba(255, 255, 255, 0.15)";
              }}
            />
          </div>

          {/* Password Input */}
          <div style={{ marginBottom: "24px" }}>
            <label style={{
              display: "block",
              fontSize: "13px",
              fontWeight: "600",
              color: "#f2ecff",
              marginBottom: "8px"
            }}>Password</label>
            <input
              type="password"
              placeholder="••••••••"
              value={form.password}
              onChange={(e) => setForm({ ...form, password: e.target.value })}
              required
              disabled={loading}
              style={{
                width: "100%",
                padding: "12px 14px",
                borderRadius: "12px",
                background: "rgba(255, 255, 255, 0.08)",
                border: "1px solid rgba(255, 255, 255, 0.15)",
                color: "#f2ecff",
                fontSize: "14px",
                transition: "all 0.2s ease",
                boxSizing: "border-box",
                outline: "none"
              }}
              onFocus={(e) => {
                e.target.style.background = "rgba(255, 255, 255, 0.12)";
                e.target.style.borderColor = "rgba(194, 136, 255, 0.4)";
              }}
              onBlur={(e) => {
                e.target.style.background = "rgba(255, 255, 255, 0.08)";
                e.target.style.borderColor = "rgba(255, 255, 255, 0.15)";
              }}
            />
          </div>

          {/* Register Button */}
          <button 
            type="submit" 
            disabled={loading}
            style={{
              width: "100%",
              padding: "12px 16px",
              borderRadius: "12px",
              background: "linear-gradient(135deg, #a855f7 0%, #d946ef 100%)",
              color: "#ffffff",
              border: "1px solid rgba(255, 255, 255, 0.2)",
              fontSize: "14px",
              fontWeight: "700",
              cursor: loading ? "not-allowed" : "pointer",
              transition: "all 0.2s ease",
              boxShadow: "0 12px 28px rgba(168, 85, 247, 0.18)",
              opacity: loading ? 0.7 : 1
            }}
            onMouseEnter={(e) => {
              if (!loading) {
                e.target.style.transform = "translateY(-2px)";
                e.target.style.boxShadow = "0 18px 34px rgba(168, 85, 247, 0.3)";
              }
            }}
            onMouseLeave={(e) => {
              e.target.style.transform = "translateY(0)";
              e.target.style.boxShadow = "0 12px 28px rgba(168, 85, 247, 0.18)";
            }}
          >
            {loading ? "Creating account..." : "Create Account"}
          </button>
        </form>

        {/* Divider */}
        <div style={{
          display: "flex",
          alignItems: "center",
          gap: "12px",
          marginBottom: "24px"
        }}>
          <div style={{
            flex: 1,
            height: "1px",
            background: "rgba(255, 255, 255, 0.1)"
          }}></div>
          <span style={{
            fontSize: "12px",
            color: "#cbbfe4"
          }}>or</span>
          <div style={{
            flex: 1,
            height: "1px",
            background: "rgba(255, 255, 255, 0.1)"
          }}></div>
        </div>

        {/* Login Link */}
        <div style={{
          padding: "16px",
          borderRadius: "12px",
          background: "rgba(255, 255, 255, 0.04)",
          border: "1px solid rgba(255, 255, 255, 0.08)",
          textAlign: "center"
        }}>
          <p style={{
            margin: 0,
            fontSize: "13px",
            color: "#cbbfe4"
          }}>
            Already have an account?{" "}
            <Link 
              to="/login"
              style={{
                color: "#d8b4fe",
                textDecoration: "none",
                fontWeight: "600",
                transition: "color 0.2s ease"
              }}
              onMouseEnter={(e) => e.target.style.color = "#f0e4ff"}
              onMouseLeave={(e) => e.target.style.color = "#d8b4fe"}
            >
              Sign in
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}