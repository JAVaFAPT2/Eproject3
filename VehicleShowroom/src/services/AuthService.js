import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const TOKEN_EXPIRES_AT_KEY = 'token_expires_at';
const REFRESH_EXPIRES_AT_KEY = 'refresh_expires_at';
const USER_ID_KEY = 'user_id';
const ROLE_KEY = 'role';

const AuthService = {
  // ===== Storage helpers =====
  getAccessToken() {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  },
  setAccessToken(token, expiresAt) {
    if (token) localStorage.setItem(ACCESS_TOKEN_KEY, token);
    if (expiresAt) localStorage.setItem(TOKEN_EXPIRES_AT_KEY, expiresAt);
  },
  getRefreshToken() {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  },
  setRefreshToken(token, expiresAt) {
    if (token) localStorage.setItem(REFRESH_TOKEN_KEY, token);
    if (expiresAt) localStorage.setItem(REFRESH_EXPIRES_AT_KEY, expiresAt);
  },
  setUserMeta({ userId, role }) {
    if (userId) localStorage.setItem(USER_ID_KEY, userId);
    if (role) localStorage.setItem(ROLE_KEY, role);
  },
  clearSession() {
    [ACCESS_TOKEN_KEY, REFRESH_TOKEN_KEY, TOKEN_EXPIRES_AT_KEY, REFRESH_EXPIRES_AT_KEY, USER_ID_KEY, ROLE_KEY]
      .forEach((key) => localStorage.removeItem(key));
  },

  // ===== API =====
  async login({ username, password }) {
    const { data } = await ApiClient.post(ApiUrl.AUTH.LOGIN, { username, password });

    // ✅ Lưu session
    this.setAccessToken(data.token, data.tokenExpiresAt);
    this.setRefreshToken(data.refreshToken, data.refreshTokenExpiresAt);
    this.setUserMeta({ userId: data.userId, role: data.role });

    return data;
  },

  async register({ username, password, email, name, phone, address }) {
    const { data } = await ApiClient.post(ApiUrl.AUTH.REGISTER, {
      username,
      password,
      email,
      name,
      phone,
      address,
    });
    return data;
  },

  async refreshToken() {
    const rt = this.getRefreshToken();
    const { data } = await ApiClient.post(ApiUrl.AUTH.REFRESH_TOKEN, { refreshToken: rt });
    this.setAccessToken(data.token, data.tokenExpiresAt);
    this.setRefreshToken(data.refreshToken, data.refreshTokenExpiresAt);
    this.setUserMeta({ userId: data.userId, role: data.role });
    return data.token;
  },

  async revokeToken() {
    const rt = this.getRefreshToken();
    if (!rt) return;
    try {
      await ApiClient.post(ApiUrl.AUTH.REVOKE_TOKEN, { refreshToken: rt });
    } catch {}
  },

  async logout() {
    await this.revokeToken();
    this.clearSession();
  },
};

export default AuthService;
