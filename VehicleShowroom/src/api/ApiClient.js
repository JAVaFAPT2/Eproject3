import axios from 'axios';
import qs from 'qs';
import AuthService from 'services/AuthService';
import { BASE_URL } from 'constants/ApiBaseUrl';

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
  if (token) {
    config.headers['Authorization'] = token.startsWith('Bearer ')
      ? token
      : `Bearer ${token}`;
  }
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
        processQueue(err, null);
        isRefreshing = false;
        console.error('❌ Refresh token failed:', err);

        // ✅ Check if user was logged in before logging out
        const hadToken = !!AuthService.getAccessToken();

        // 🚪 Logout (clear tokens)
        AuthService.logout();

        // ✅ Only redirect if user was logged in
        if (hadToken) {
          window.location.href = '/auth/sign-in';
        }

        return Promise.reject(err);
      }
    },
  );

// ✅ Gắn interceptor refresh cho cả 2 client
createResponseInterceptor(ApiClient);
createResponseInterceptor(uploadClient);

export default ApiClient;
