// services/OrderService.js
import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const OrderService = {
  // 🟢 Lấy danh sách Orders
  get: async (params = {}) => {
    const response = await ApiClient.get(ApiUrl.ORDERS.BASE, {
      params,
    });
    return response.data;
  },

  getById: async (id) => {
    const res = await ApiClient.get(ApiUrl.ORDERS.BY_ID(id));
    return res.data;
  },

  // 🟣 Tạo Order mới
  create: async (data) => {
    const res = await ApiClient.post(ApiUrl.ORDERS.BASE, data);
    return res.data;
  },

  // 🔹 Gán vehicle cho order
  assignVehicle: async (orderId, vehicleId, dealerId) => {
    const res = await ApiClient.post(ApiUrl.ORDERS.ASSIGN_VEHICLE(orderId), {
      vehicleId,
      dealerId,
    });
    return res.data;
  },

  updateStatus: async (id, status) => {
    const res = await ApiClient.put(ApiUrl.ORDERS.STATUS(id), {
      status,
    });
    return res.data;
  },
};

export default OrderService;
