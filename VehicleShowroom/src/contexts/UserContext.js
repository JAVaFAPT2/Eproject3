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
  const [isAuthenticated, setIsAuthenticated] = useState(
    !!AuthService.getAccessToken(),
  );

  const fetchUserProfile = useCallback(async () => {
    try {
      const token = AuthService.getAccessToken();
      if (!token) {
        setUser(null);
        setIsAuthenticated(false);
        setLoading(false);
        return;
      }
      const profile = await ProfileService.getProfile();
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

  const refreshUser = async () => {
    await fetchUserProfile();
  };

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
      value={{ user, setUser, isAuthenticated, loading, refreshUser, logout }}
    >
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
