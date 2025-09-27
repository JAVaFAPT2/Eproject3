import axios from 'axios';
import AuthService from 'services/AuthService';

const ApiClient = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:8080/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

ApiClient.interceptors.request.use(
  (config) => {
    const token = AuthService.getAccessToken();
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

ApiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    const hasToken = !!AuthService.getAccessToken();
    if (
      (error.response?.status === 401 || error.response?.status === 403) &&
      hasToken &&
      !originalRequest._retry
    ) {
      originalRequest._retry = true;
      try {
        const newAccessToken = await AuthService.refreshToken();
        originalRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
        return ApiClient(originalRequest);
      } catch {
        AuthService.logout();
      }
    }

    if (!hasToken && error.response?.status === 403) {
      return Promise.resolve({ data: null });
    }

    return Promise.reject(error);
  },
);

export default ApiClient;
