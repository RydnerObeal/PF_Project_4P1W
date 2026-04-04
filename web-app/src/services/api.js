import axios from "axios";

// Auth API (port 5068) and Resource API (port 5208)
const authApi = axios.create({
  baseURL: "http://localhost:5068/api",
});

const api = axios.create({
  baseURL: "http://localhost:5208/api",
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