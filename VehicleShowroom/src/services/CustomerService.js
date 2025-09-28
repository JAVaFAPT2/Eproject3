import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const CustomerService = {
  getAll: (params) => ApiClient.get(ApiUrl.CUSTOMERS.BASE, { params }),
  getOrders: (id) => ApiClient.get(ApiUrl.CUSTOMERS.ORDERS(id)),
};

export default CustomerService;
