import axios from "axios";

const getLocalhostBaseUrl = (httpPort, httpsPort) => {
  if (typeof window === "undefined") {
    return `http://localhost:${httpPort}/api`;
  }

  return window.location.protocol === "https:"
    ? `https://localhost:${httpsPort}/api`
    : `http://localhost:${httpPort}/api`;
};

const authApi = axios.create({
  baseURL: import.meta.env.VITE_AUTH_API_URL || getLocalhostBaseUrl(5068, 7182),
});

const api = axios.create({
  baseURL: import.meta.env.VITE_RESOURCE_API_URL || getLocalhostBaseUrl(5208, 7180),
});

// Interceptor for both to add auth token
const tokenInterceptor = (config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
};

authApi.interceptors.request.use(tokenInterceptor);
api.interceptors.request.use(tokenInterceptor);

export const getTopScorer = () => api.get('/game/top-scorer');
export const getUserById = (id) => authApi.get(`/auth/user/${id}`);

export { authApi, api };
export default api;