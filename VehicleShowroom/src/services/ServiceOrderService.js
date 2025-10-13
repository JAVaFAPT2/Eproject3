import { serviceOrders } from '../mockData/serviceOrders.js';
import { simulateDelay } from './utils.js';

const ServiceOrderService = {
  getAll: () => simulateDelay(serviceOrders),
  getById: (id) => simulateDelay(serviceOrders.find((s) => s.serviceOrderId === id)),

  create: (data) => {
    const newSO = { ...data, serviceOrderId: `so${serviceOrders.length + 1}` };
    serviceOrders.push(newSO);
    return simulateDelay({ message: 'Service order created successfully', data: newSO });
  },

  updateStatus: (id, status) => {
    const s = serviceOrders.find((x) => x.serviceOrderId === id);
    if (s) s.status = status;
    return simulateDelay({ message: 'Service order status updated successfully' });
  },
};

export default ServiceOrderService;
