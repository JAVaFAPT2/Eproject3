import { users } from '../mockData/users.js';
import { simulateDelay } from './utils.js';

const ProfileService = {
  getProfile: (userId) => simulateDelay(users.find((u) => u.userId === userId)),

  updateProfile: (userId, data) => {
    const i = users.findIndex((u) => u.userId === userId);
    if (i >= 0) users[i] = { ...users[i], ...data, updatedAt: new Date().toISOString() };
    return simulateDelay({ message: 'Profile updated successfully' });
  },

  changePassword: (userId, { currentPassword, newPassword }) => {
    const user = users.find((u) => u.userId === userId);
    if (!user || user.passwordHash !== currentPassword)
      return simulateDelay({ message: 'Invalid current password' });
    user.passwordHash = newPassword;
    return simulateDelay({ message: 'Password changed successfully' });
  },
};

export default ProfileService;
