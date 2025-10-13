import { orders } from '../mockData/orders.js';
import { simulateDelay } from './utils.js';

const OrderService = {
  getAll: (filter = {}) => {
    let data = [...orders];
    if (filter.status) data = data.filter((o) => o.status === filter.status);
    if (filter.customerId) data = data.filter((o) => o.customerId === filter.customerId);
    return simulateDelay(data);
  },

  getById: (id) => simulateDelay(orders.find((o) => o.orderId === id)),

  create: (data) => {
    const newOrder = { ...data, orderId: `o${orders.length + 1}` };
    orders.push(newOrder);
    return simulateDelay({ message: 'Order created successfully', data: newOrder });
  },

  assignVehicle: (id, vehicleId) => {
    const o = orders.find((x) => x.orderId === id);
    if (o) o.vehicleId = vehicleId;
    return simulateDelay({ message: 'Vehicle assigned successfully' });
  },

  updateStatus: (id, status) => {
    const o = orders.find((x) => x.orderId === id);
    if (o) o.status = status;
    return simulateDelay({ message: 'Order status updated successfully' });
  },
};

export default OrderService;
