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
  login: async () => {},
  logout: async () => {},
  refreshUser: async () => {},
});

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(
    !!AuthService.getAccessToken(),
  );

  // 🧩 Load user profile
  const fetchUserProfile = useCallback(async () => {
    try {
      const token = AuthService.getAccessToken();
      if (!token) {
        setUser(null);
        setIsAuthenticated(false);
        setLoading(false);
        return;
      }

      const profile = await ProfileService.get();
      setUser(profile);
      setIsAuthenticated(true);
    } catch (error) {
      console.warn('❌ Failed to fetch profile:', error);
      setUser(null);
      setIsAuthenticated(false);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchUserProfile();
  }, [fetchUserProfile]);

  // 🟢 Login (thay vì gọi trực tiếp AuthService.login trong component)
  const login = async (credentials, keepLoggedIn = false) => {
    const res = await AuthService.login(credentials, keepLoggedIn);
    const { user } = res;
    setUser(user);
    setIsAuthenticated(true);
    return user;
  };

  const refreshUser = async () => {
    await fetchUserProfile();
  };

  // 🟣 Logout
  const logout = async () => {
    try {
      await AuthService.logout();
    } catch (err) {
      console.warn('Logout failed:', err);
    } finally {
      setUser(null);
      setIsAuthenticated(false);
    }
  };

  return (
    <UserContext.Provider
      value={{
        user,
        isAuthenticated,
        loading,
        login,
        logout,
        refreshUser,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
