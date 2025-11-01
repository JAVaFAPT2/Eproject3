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

  // 🟢 Cập nhật trạng thái đơn hàng (PUT /api/PurchaseOrders/{id}/status)
  updateStatus: async (orderId, status) => {
    const res = await ApiClient.put(ApiUrl.PURCHASE_ORDERS.STATUS(orderId), {
      status,
    });
    return res.data;
  },
};

export default PurchaseOrderService;
