import React, {
  createContext,
  useContext,
  useEffect,
  useState,
  useCallback,
} from 'react';
import AuthService from 'services/AuthService';
import ProfileService from 'services/ProfileService';

const UserContext = createContext({
  user: null,
  isAuthenticated: false,
  loading: true,
  setUser: () => {},
  refreshUser: () => {},
  logout: () => {},
});

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const isAuthenticated = !!AuthService.getAccessToken();

  // 🧠 Lấy hồ sơ người dùng từ API
  const fetchUserProfile = useCallback(async () => {
    try {
      const token = AuthService.getAccessToken();
      if (!token) {
        setUser(null);
        setLoading(false);
        return;
      }

      const profile = await ProfileService.getProfile();
      setUser(profile);
    } catch (error) {
      console.warn('❌ Failed to fetch profile:', error);
      setUser(null);
    } finally {
      setLoading(false);
    }
  }, []);

  // 🪄 Khi mount hoặc token thay đổi
  useEffect(() => {
    fetchUserProfile();
  }, [fetchUserProfile]);

  // 🔁 Cho phép gọi lại thủ công (vd: sau khi update profile)
  const refreshUser = async () => {
    await fetchUserProfile();
  };

  // 🚪 Logout: clear token + user
  const logout = async () => {
    try {
      await AuthService.logout();
    } catch (err) {
      console.warn('Logout failed:', err);
    } finally {
      setUser(null);
    }
  };

  return (
    <UserContext.Provider
      value={{
        user,
        setUser,
        isAuthenticated,
        loading,
        refreshUser,
        logout,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
