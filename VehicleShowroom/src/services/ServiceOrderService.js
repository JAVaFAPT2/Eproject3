import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const ServiceOrderService = {
  getAll: (params) => ApiClient.get(ApiUrl.SERVICE_ORDERS.BASE, { params }),
  create: (data) => ApiClient.post(ApiUrl.SERVICE_ORDERS.BASE, data),
  start: (id) => ApiClient.put(ApiUrl.SERVICE_ORDERS.START(id)),
};

export default ServiceOrderService;
