import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

// ================================
// 🔐 AUTH SERVICE
// ================================
const ACCESS_TOKEN_KEY = 'accessToken';    
const REFRESH_TOKEN_KEY = 'refreshToken';
const KEEP_LOGIN_KEY = 'keepLoggedIn';

const AuthService = {
  // ---------------------------------
  // 🧩 Token helpers
  // ---------------------------------
  setAccessToken(token, keepLoggedIn = null) {
    const keep = keepLoggedIn ?? AuthService.isKeepLoggedIn(); // nếu không truyền thì lấy theo state hiện tại

    if (keep) {
      localStorage.setItem(ACCESS_TOKEN_KEY, token);
    } else {
      sessionStorage.setItem(ACCESS_TOKEN_KEY, token);
    }
  },

  getAccessToken() {
    return (
      localStorage.getItem(ACCESS_TOKEN_KEY) ||
      sessionStorage.getItem(ACCESS_TOKEN_KEY)
    );
  },

  setRefreshToken(token, keepLoggedIn = null) {
    const keep = keepLoggedIn ?? AuthService.isKeepLoggedIn();

    if (keep) {
      localStorage.setItem(REFRESH_TOKEN_KEY, token);
    } else {
      sessionStorage.setItem(REFRESH_TOKEN_KEY, token);
    }
  },

  getRefreshToken() {
    return (
      localStorage.getItem(REFRESH_TOKEN_KEY) ||
      sessionStorage.getItem(REFRESH_TOKEN_KEY)
    );
  },

  clearTokens() {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    sessionStorage.removeItem(ACCESS_TOKEN_KEY);
    sessionStorage.removeItem(REFRESH_TOKEN_KEY);
  },

  // ---------------------------------
  // 🧠 Keep login state
  // ---------------------------------
  setKeepLoggedIn(value) {
    localStorage.setItem(KEEP_LOGIN_KEY, value ? 'true' : 'false');
  },

  isKeepLoggedIn() {
    return localStorage.getItem(KEEP_LOGIN_KEY) === 'true';
  },

  // ---------------------------------
  // 🧍 Register
  // ---------------------------------
  register: async (data) => {
    const res = await ApiClient.post(ApiUrl.AUTH.REGISTER, data);
    return res.data;
  },

  // ---------------------------------
  // 🔑 Login
  // ---------------------------------
  login: async (data, keepLoggedIn = false) => {
    const res = await ApiClient.post(ApiUrl.AUTH.LOGIN, data);
    const { token, refreshToken } = res.data;

    AuthService.setKeepLoggedIn(keepLoggedIn);

    // ✅ Save tokens correctly
    if (token) AuthService.setAccessToken(token, keepLoggedIn);
    if (refreshToken) AuthService.setRefreshToken(refreshToken, keepLoggedIn);

    return res.data;
  },

  // ---------------------------------
  // 🔁 Refresh token (used by ApiClient interceptor)
  // ---------------------------------
  refreshToken: async () => {
    const refreshToken = AuthService.getRefreshToken();
    if (!refreshToken) throw new Error('No refresh token found');

    const res = await ApiClient.post(ApiUrl.AUTH.REFRESH_TOKEN, {
      refreshToken,
    });

    const keepLoggedIn = AuthService.isKeepLoggedIn();

    if (res.data?.accessToken)
      AuthService.setAccessToken(res.data.accessToken, keepLoggedIn);
    if (res.data?.refreshToken)
      AuthService.setRefreshToken(res.data.refreshToken, keepLoggedIn);

    return res.data?.accessToken;
  },

  // ---------------------------------
  // ❌ Logout / Revoke token
  // ---------------------------------
  logout: async () => {
    const refreshToken = AuthService.getRefreshToken();
    if (refreshToken) {
      try {
        await ApiClient.post(ApiUrl.AUTH.REVOKE_TOKEN, { refreshToken });
      } catch (err) {
        console.warn('Revoke token failed', err);
      }
    }

    AuthService.clearTokens();
    localStorage.removeItem(KEEP_LOGIN_KEY);
  },

  // ---------------------------------
  // 📧 Forgot Password
  // ---------------------------------
  forgotPassword: async (email) => {
    const res = await ApiClient.post(ApiUrl.AUTH.FORGOT_PASSWORD, { email });
    return res.data;
  },

  // ---------------------------------
  // 🔑 Reset Password
  // ---------------------------------
  resetPassword: async (token, newPassword) => {
    const res = await ApiClient.post(ApiUrl.AUTH.RESET_PASSWORD, {
      token,
      newPassword,
    });
    return res.data;
  },
};

export default AuthService;
