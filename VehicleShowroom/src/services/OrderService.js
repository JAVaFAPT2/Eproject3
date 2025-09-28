import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const OrderService = {
  getAll: (params) => ApiClient.get(ApiUrl.ORDERS.BASE, { params }),
  getById: (id) => ApiClient.get(ApiUrl.ORDERS.BY_ID(id)),
  create: (data) => ApiClient.post(ApiUrl.ORDERS.BASE, data),
  print: (id) => ApiClient.post(ApiUrl.ORDERS.PRINT(id)),
};

export default OrderService;
