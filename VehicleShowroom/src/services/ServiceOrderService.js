// services/OrderService.js
import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const ServiceOrderService = {
  get: async (params = {}) => {
    const response = await ApiClient.get(ApiUrl.SERVICE_ORDERS.BASE, {
      params,
    });
    return response.data;
  },

  create: async (data) => {
    const response = await ApiClient.post(ApiUrl.SERVICE_ORDERS.BASE, data);
    return response.data;
  },
  
  updateStatus: async (id, payload) => {
    const response = await ApiClient.put(
      ApiUrl.SERVICE_ORDERS.STATUS(id),
      payload,
    );
    return response.data;
  },
};

export default ServiceOrderService;
