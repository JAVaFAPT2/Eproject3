import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const PurchaseOrderService = {
  getAll: (params) => ApiClient.get(ApiUrl.PURCHASE_ORDERS.BASE, { params }),
  getById: (id) => ApiClient.get(ApiUrl.PURCHASE_ORDERS.BY_ID(id)),
  create: (data) => ApiClient.post(ApiUrl.PURCHASE_ORDERS.BASE, data),
  approve: (id) => ApiClient.put(ApiUrl.PURCHASE_ORDERS.APPROVE(id)),
};

export default PurchaseOrderService;
