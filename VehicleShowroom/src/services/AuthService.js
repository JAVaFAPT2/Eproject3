import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const TOKEN_KEY = 'accessToken';
const TOKEN_EXPIRES_KEY = 'tokenExpiresAt';

// ==================
// 🔑 Token Helpers
// ==================
const getAccessToken = () => localStorage.getItem(TOKEN_KEY);

const setAccessToken = (token) => {
  if (!token) {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(TOKEN_EXPIRES_KEY);
  } else {
    localStorage.setItem(TOKEN_KEY, token);
  }
};

const setTokenExpiration = (expiresAt) => {
  if (expiresAt) localStorage.setItem(TOKEN_EXPIRES_KEY, expiresAt);
};

const clearAuth = () => {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(TOKEN_EXPIRES_KEY);
};

// ==================
// 🧩 AuthService
// ==================
const AuthService = {
  // 1️⃣ Login
  async login(username, password) {
    const res = await ApiClient.post(ApiUrl.AUTH.LOGIN, { username, password });
    const { token, tokenExpiresAt } = res.data;

    setAccessToken(token);
    setTokenExpiration(tokenExpiresAt);

    return res.data;
  },

  // 2️⃣ Forgot Password
  async forgotPassword(email) {
    const res = await ApiClient.post(ApiUrl.AUTH.FORGOT_PASSWORD, { email });
    return res.data;
  },

  // 3️⃣ Reset Password
  async resetPassword(token, newPassword) {
    const res = await ApiClient.post(ApiUrl.AUTH.RESET_PASSWORD, {
      token,
      newPassword,
    });
    return res.data;
  },

  // 4️⃣ Refresh Token
  async refreshToken() {
    const res = await ApiClient.post(ApiUrl.AUTH.REFRESH_TOKEN, null, {
      withCredentials: true, // refresh token nằm trong HttpOnly cookie
    });

    const { token, tokenExpiresAt } = res.data;
    setAccessToken(token);
    setTokenExpiration(tokenExpiresAt);

    return token;
  },

  // 5️⃣ Register
  async register({ username, password, email }) {
    const res = await ApiClient.post(ApiUrl.AUTH.REGISTER, {
      username,
      password,
      email,
    });
    return res.data;
  },

  // 6️⃣ Revoke Token (Logout)
  async revokeToken() {
    try {
      await ApiClient.post(ApiUrl.AUTH.REVOKE_TOKEN, null, {
        withCredentials: true,
      });
    } catch (err) {
      console.warn('Token revoke failed (maybe expired already)', err);
    } finally {
      clearAuth();
    }
  },

  // 🚪 Logout
  logout() {
    clearAuth();
  },

  // 🧭 Check login state
  isAuthenticated() {
    const token = getAccessToken();
    const expiresAt = localStorage.getItem(TOKEN_EXPIRES_KEY);

    if (!token || !expiresAt) return false;
    return new Date(expiresAt) > new Date();
  },

  getAccessToken,
  setAccessToken,
};

export default AuthService;
