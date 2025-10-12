import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

const DashboardService = {
  getRevenue: () => ApiClient.get(ApiUrl.DASHBOARD.REVENUE),
  getCustomer: () => ApiClient.get(ApiUrl.DASHBOARD.CUSTOMER),
  getTopVehicles: () => ApiClient.get(ApiUrl.DASHBOARD.TOP_VEHICLES),
  getRecentOrders: () => ApiClient.get(ApiUrl.DASHBOARD.RECENT_ORDERS),
};

export default DashboardService;
