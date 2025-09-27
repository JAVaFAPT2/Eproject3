import { createContext, useContext, useEffect, useState } from 'react';
import UserService from 'services/UserService';
import AuthService from 'services/AuthService';

const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const refreshUser = async () => {
    try {
      setLoading(true);
      const token = AuthService.getAccessToken();
      if (!token) {
        setUser(null);
        return;
      }
      const res = await UserService.getProfile();
      setUser(res);
    } catch (err) {
      if (
        err.response &&
        err.response.status !== 401 &&
        err.response.status !== 403
      ) {
        console.error('Failed to fetch user', err);
      }
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    refreshUser();
  }, []);

  return (
    <UserContext.Provider value={{ user, setUser, refreshUser, loading }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
