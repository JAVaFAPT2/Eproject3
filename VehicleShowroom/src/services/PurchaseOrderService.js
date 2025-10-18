// src/services/PurchaseOrderService.js
import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const PurchaseOrderService = {
  // 🟢 Lấy danh sách đơn hàng (có phân trang)
  get: async (params = {}) => {
    const res = await ApiClient.get(ApiUrl.PURCHASE_ORDERS.BASE, { params });
    return res.data;
  },

  // 🟢 Tạo đơn hàng (POST /api/PurchaseOrders)
  create: async (data) => {
    const res = await ApiClient.post(ApiUrl.PURCHASE_ORDERS.BASE, data);
    return res.data;
  },

  // 🟢 Thêm dòng hàng (POST /api/PurchaseOrders/{id}/lines)
  addLine: async (orderId, lineData) => {
    const res = await ApiClient.post(
      ApiUrl.PURCHASE_ORDERS.LINES(orderId),
      lineData,
    );
    return res.data;
  },

  // 🟢 Hoàn tất đơn hàng (POST /api/PurchaseOrders/{id}/complete)
  complete: async (orderId) => {
    const res = await ApiClient.post(ApiUrl.PURCHASE_ORDERS.COMPLETE(orderId));
    return res.data;
  },
};

export default PurchaseOrderService;
