import { orders } from '../mockData/orders.js';
import { vehicleModels } from '../mockData/vehicleModels.js';
import { simulateDelay } from './utils.js';

const DashboardService = {
  getRevenue: () => {
    const totalRevenue = orders.reduce((a, b) => a + b.salePrice, 0);
    return simulateDelay({
      totalRevenue,
      monthlyRevenue: totalRevenue / 12,
      yearlyRevenue: totalRevenue,
      revenueGrowth: 15.5,
      topSellingModels: vehicleModels.map((v) => ({
        modelNumber: v.modelNumber,
        name: v.name,
        brand: v.brand,
        totalSold: 5,
        revenue: v.price * 5,
      })),
    });
  },
};

export default DashboardService;
