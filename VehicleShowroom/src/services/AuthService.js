import { users } from '../mockData/users.js';
import { roles } from '../mockData/roles.js';
import { simulateDelay, simulateReject } from './utils.js';

let currentToken = null;

const AuthService = {
  login: async ({ username, password }) => {
    const user = users.find(
      (u) => u.username === username && u.passwordHash === password
    );
    if (!user) return simulateReject({ message: 'Invalid credentials' });

    const token = `token-${user.userId}`;
    currentToken = token;

    return simulateDelay({
      token,
      refreshToken: `refresh-${user.userId}`,
      tokenExpiresAt: new Date(Date.now() + 3600_000).toISOString(),
      refreshTokenExpiresAt: new Date(Date.now() + 7 * 86400_000).toISOString(),
      userId: user.userId,
      role: roles.find((r) => r.roleId === user.roleId)?.name || 'Unknown',
      message: 'Login successful',
    });
  },

  register: async (data) => {
    const newUser = {
      userId: `u${users.length + 1}`,
      ...data,
      roleId: roles.find((r) => r.name === 'Customer')?.roleId,
      status: 'Active',
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    };
    users.push(newUser);
    return simulateDelay({ id: newUser.userId, message: 'User registered successfully' });
  },

  refreshToken: async () =>
    simulateDelay({ token: currentToken || 'mock-token', message: 'Token refreshed successfully' }),

  logout: async () => simulateDelay({ message: 'Logged out' }),
};

export default AuthService;
