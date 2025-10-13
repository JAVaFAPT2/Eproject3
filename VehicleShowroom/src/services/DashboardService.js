import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const DashboardService = {
  getRevenue() {
    return ApiClient.get(ApiUrl.DASHBOARD.REVENUE).then(r => r.data);
  },
  getCustomer() {
    return ApiClient.get(ApiUrl.DASHBOARD.CUSTOMER).then(r => r.data);
  },
  getTopVehicles() {
    return ApiClient.get(ApiUrl.DASHBOARD.TOP_VEHICLES).then(r => r.data);
  },
  getRecentOrders() {
    return ApiClient.get(ApiUrl.DASHBOARD.RECENT_ORDERS).then(r => r.data);
  },
};

export default DashboardService;
