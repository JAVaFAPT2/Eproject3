import { GoogleAuthProvider, signInWithPopup } from 'firebase/auth';
import { auth } from '../firebase/firebase';
import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';
import { jwtDecode } from 'jwt-decode';

class AuthService {
  constructor() {
    this.keepLoggedIn = false;
  }

  setKeepLoggedIn(value) {
    this.keepLoggedIn = value;
  }

  saveTokens(accessToken, refreshToken) {
    this.removeTokens();
    if (this.keepLoggedIn) {
      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('refreshToken', refreshToken);
    } else {
      sessionStorage.setItem('accessToken', accessToken);
      sessionStorage.setItem('refreshToken', refreshToken);
    }
  }

  getAccessToken() {
    return (
      localStorage.getItem('accessToken') ||
      sessionStorage.getItem('accessToken')
    );
  }

  getRefreshToken() {
    return (
      localStorage.getItem('refreshToken') ||
      sessionStorage.getItem('refreshToken')
    );
  }

  removeTokens() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    sessionStorage.removeItem('accessToken');
    sessionStorage.removeItem('refreshToken');
  }

  async emailLogin({ email, password, keepLoggedIn }) {
    this.setKeepLoggedIn(keepLoggedIn);

    // ❌ Không gửi keepLoggedIn lên BE nữa
    const response = await ApiClient.post(ApiUrl.AUTH_LOGIN, {
      email,
      password,
    });

    const { accessToken, refreshToken } = response.data;
    this.saveTokens(accessToken, refreshToken);
    return response.data;
  }

  async googleLogin({ keepLoggedIn }) {
    this.setKeepLoggedIn(keepLoggedIn);
    const provider = new GoogleAuthProvider();
    const result = await signInWithPopup(auth, provider);
    const idToken = await result.user.getIdToken();

    // ❌ Không gửi keepLoggedIn lên BE
    const response = await ApiClient.post(ApiUrl.AUTH_SOCIAL_LOGIN, {
      idToken,
      provider: 'GOOGLE',
    });

    const { accessToken, refreshToken } = response.data;
    this.saveTokens(accessToken, refreshToken);
    return response.data;
  }

  async signUp({ fullName, email, password }) {
    return ApiClient.post(ApiUrl.AUTH_REGISTER, {
      fullName,
      email,
      password,
    });
  }

  async getProfile() {
    try {
      const response = await ApiClient.get(ApiUrl.PROFILE);
      return response.data;
    } catch (error) {
      console.error('Error fetching profile:', error);
      throw error;
    }
  }

  async logout() {
    this.removeTokens();
  }

  async refreshToken() {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) throw new Error('No refresh token available');

    const response = await ApiClient.post(ApiUrl.AUTH_REFRESH, {
      refreshToken,
    });
    const { accessToken } = response.data;
    this.saveTokens(accessToken, refreshToken);
    return accessToken;
  }

  async forgotPassword(email) {
    return ApiClient.post(ApiUrl.AUTH_FORGOT_PASSWORD, { email });
  }

  async resetPassword({ token, password }) {
    return ApiClient.post(ApiUrl.AUTH_RESET_PASSWORD, {
      token,
      newPassword: password,
    });
  }

  getDecodedAccessToken() {
    const token = this.getAccessToken();
    if (!token) return null;
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  getRole() {
    const decoded = this.getDecodedAccessToken();
    return decoded?.role || null;
  }
}

const authService = new AuthService();
export default authService;
