import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const UserService = {
  getAll: async (params = {}) => {
    const res = await ApiClient.get(ApiUrl.USERS.BASE, { params });
    return res.data;
  },

  create: async (data) => {
    const payload = {
      username: data.username,
      email: data.email,
      password: data.password,
      name: data.name,
      phone: data.phone || '',
      address: data.address || '',
      hireDate: new Date().toISOString(),
    };

    const res = await ApiClient.post(ApiUrl.USERS.BASE, payload);
    return res.data;
  },

  toggleActive: async (id, isActive) => {
    const res = await ApiClient.patch(`${ApiUrl.USERS.BASE}/${id}`, {
      isActive,
    });
    return res.data;
  },
};

export default UserService;
