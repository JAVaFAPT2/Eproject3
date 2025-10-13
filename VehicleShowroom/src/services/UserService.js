import { users } from '../mockData/users.js';
import { simulateDelay } from './utils.js';

const UserService = {
  getAll: (filter = {}) => {
    let data = [...users];
    if (filter.roleId) data = data.filter((u) => u.roleId === filter.roleId);
    if (filter.searchTerm)
      data = data.filter((u) =>
        u.name.toLowerCase().includes(filter.searchTerm.toLowerCase())
      );
    return simulateDelay(data);
  },

  getById: (id) => simulateDelay(users.find((u) => u.userId === id)),

  create: (data) => {
    const newUser = {
      userId: `u${users.length + 1}`,
      ...data,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      status: 'Active',
    };
    users.push(newUser);
    return simulateDelay({ message: 'User created successfully', data: newUser });
  },

  update: (id, data) => {
    const i = users.findIndex((u) => u.userId === id);
    if (i >= 0) users[i] = { ...users[i], ...data };
    return simulateDelay({ message: 'User updated successfully' });
  },

  delete: (id) => {
    const i = users.findIndex((u) => u.userId === id);
    if (i >= 0) users.splice(i, 1);
    return simulateDelay({ message: 'User deleted successfully' });
  },
};

export default UserService;
