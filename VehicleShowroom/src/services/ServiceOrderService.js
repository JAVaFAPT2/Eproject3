import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const ServiceOrderService = {
  create(payload) {
    // { orderId, createdBy, type, cost, appointmentDate, description }
    return ApiClient.post(ApiUrl.SERVICE_ORDERS.BASE, payload).then(r => r.data);
  },
  updateStatus(id, status) {
    // status: 1 Scheduled, 2 Completed, 3 Cancelled
    return ApiClient.put(ApiUrl.SERVICE_ORDERS.STATUS(id), { status }).then(r => r.data);
  },
};

export default ServiceOrderService;
