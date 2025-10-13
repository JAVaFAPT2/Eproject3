import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const OrderService = {
  create(payload) {
    // { customerId, dealerId, modelNumber, salePrice, vehicleId, appointmentDate, note }
    return ApiClient.post(ApiUrl.ORDERS.BASE, payload).then(r => r.data);
  },
  getById(id) {
    return ApiClient.get(ApiUrl.ORDERS.BY_ID(id)).then(r => r.data);
  },
  assignVehicle(id, vehicleId) {
    return ApiClient.post(ApiUrl.ORDERS.ASSIGN_VEHICLE(id), { vehicleId }).then(r => r.data);
  },
  confirm(id) {
    return ApiClient.post(ApiUrl.ORDERS.CONFIRM(id)).then(r => r.data);
  },
  complete(id) {
    return ApiClient.post(ApiUrl.ORDERS.COMPLETE(id)).then(r => r.data);
  },
};

export default OrderService;
