import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const DashboardService = {
  getOverview: async () => {
    const res = await ApiClient.get(ApiUrl.DASHBOARD.OVERVIEW);
    return res.data;
  },
  getRevenue: async () => {
    const res = await ApiClient.get(ApiUrl.DASHBOARD.REVENUE);
    return res.data;
  },
  getCustomer: async () => {
    const res = await ApiClient.get(ApiUrl.DASHBOARD.CUSTOMER);
    return res.data;
  },
  getTopVehicles: async () => {
    const res = await ApiClient.get(ApiUrl.DASHBOARD.TOP_VEHICLES);
    return res.data;
  },
  getRecentOrders: async () => {
    const res = await ApiClient.get(ApiUrl.DASHBOARD.RECENT_ORDERS);
    return res.data;
  },
};

export default DashboardService;
