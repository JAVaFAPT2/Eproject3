// services/OrderService.js
import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const OrderService = {
  // 🟢 Lấy danh sách Orders
  get: async () => {
    const res = await ApiClient.get(ApiUrl.ORDERS.BASE);
    return res.data;
  },

  // 🟣 Tạo Order mới
  create: async (data) => {
    const res = await ApiClient.post(ApiUrl.ORDERS.BASE, data);
    return res.data;
  },

  // 🔹 Gán vehicle cho order
  assignVehicle: async (orderId, vehicleId) => {
    const res = await ApiClient.post(ApiUrl.ORDERS.ASSIGN_VEHICLE(orderId), {
      vehicleId,
    });
    return res.data;
  },

  updateStatus: async (id, status) => {
    const res = await ApiClient.put(`${ApiUrl.ORDERS.BASE}/${id}/status`, {
      status,
    });
    return res.data;
  },
};

export default OrderService;
