import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const TOKEN_KEY = 'accessToken';
const REFRESH_KEY = 'refreshToken';
const KEEP_LOGIN_KEY = 'keepLogin';

const AuthService = {
  async login(credentials, keepLogin = false) {
    const res = await ApiClient.post(ApiUrl.AUTH.LOGIN, credentials);
    const { token, refreshToken } = res.data;

    const storage = keepLogin ? localStorage : sessionStorage;
    storage.setItem(TOKEN_KEY, token);
    storage.setItem(REFRESH_KEY, refreshToken);

    localStorage.setItem(KEEP_LOGIN_KEY, keepLogin ? 'true' : 'false');

    return res.data;
  },

  async refreshToken() {
    const keepLogin = localStorage.getItem(KEEP_LOGIN_KEY) === 'true';
    const storage = keepLogin ? localStorage : sessionStorage;

    const refreshToken = storage.getItem(REFRESH_KEY);
    if (!refreshToken) throw new Error('No refresh token found');

    const res = await ApiClient.post(ApiUrl.AUTH.REFRESH_TOKEN, {
      refreshToken,
    });

    storage.setItem(TOKEN_KEY, res.data.token);
    storage.setItem(REFRESH_KEY, res.data.refreshToken);

    return res.data.token;
  },

  getAccessToken() {
    const keepLogin = localStorage.getItem(KEEP_LOGIN_KEY) === 'true';
    const storage = keepLogin ? localStorage : sessionStorage;
    return storage.getItem(TOKEN_KEY);
  },

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_KEY);
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(REFRESH_KEY);
    localStorage.removeItem(KEEP_LOGIN_KEY);
  },
};

export default AuthService;
