import axios from 'axios';
import qs from 'qs';
import AuthService from 'services/AuthService';

const BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:8080/api';

// ✅ Serialize query param (color=red&color=blue)
const paramsSerializer = (params) =>
  qs.stringify(params, { arrayFormat: 'repeat' });
// ==================
// 📦 ApiClient (JSON)
// ==================
const ApiClient = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': 'application/json' },
  paramsSerializer,
});

// ==================
// 📦 UploadClient (FormData)
// ==================
export const uploadClient = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': 'multipart/form-data' },
  maxContentLength: Infinity,
  maxBodyLength: Infinity,
  paramsSerializer,
});

// ==================
// 🔑 Request Interceptor (Attach Token)
// ==================
const attachToken = (config) => {
  const token = AuthService.getAccessToken();
  if (token) config.headers['Authorization'] = `Bearer ${token}`;
  return config;
};

ApiClient.interceptors.request.use(attachToken);
uploadClient.interceptors.request.use(attachToken);

// ==================
// 🔁 Response Interceptor (Handle 401 + 403 → Refresh Token)
// ==================
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) prom.reject(error);
    else prom.resolve(token);
  });
  failedQueue = [];
};

const shouldRefresh = (error) => {
  const status = error.response?.status;
  return (status === 401 || status === 403) && !error.config._retry;
};

const createResponseInterceptor = (client) =>
  client.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;

      // ❌ Không phải lỗi cần refresh
      if (!shouldRefresh(error)) {
        return Promise.reject(error);
      }

      // 🌀 Nếu đang refresh → đợi token mới
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({
            resolve: (token) => {
              originalRequest.headers['Authorization'] = `Bearer ${token}`;
              resolve(client(originalRequest));
            },
            reject,
          });
        });
      }

      // 🪄 Đánh dấu retry
      originalRequest._retry = true;
      isRefreshing = true;

      try {
        // 🌟 Gọi refresh token
        const newToken = await AuthService.refreshToken();

        // ✅ Lưu token mới lại
        AuthService.setAccessToken(newToken);

        // ✅ Update default header
        ApiClient.defaults.headers.common[
          'Authorization'
        ] = `Bearer ${newToken}`;
        uploadClient.defaults.headers.common[
          'Authorization'
        ] = `Bearer ${newToken}`;

        // ✅ Cập nhật queue
        processQueue(null, newToken);
        isRefreshing = false;

        // ✅ Gắn token mới và retry request
        originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
        return client(originalRequest);
      } catch (err) {
        // ❌ Refresh thất bại → clear queue, logout
        processQueue(err, null);
        isRefreshing = false;
        console.error('❌ Refresh token failed:', err);

        // 🚪 Logout và redirect sign-in
        AuthService.logout();
        window.location.href = '/auth/sign-in';
        return Promise.reject(err);
      }
    },
  );

// ✅ Gắn interceptor refresh cho cả 2 client
createResponseInterceptor(ApiClient);
createResponseInterceptor(uploadClient);

export default ApiClient;
