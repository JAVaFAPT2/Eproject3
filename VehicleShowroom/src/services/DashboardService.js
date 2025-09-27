import ApiClient from "api/ApiClient";
import ApiUrl from "constant/ApiUrl";

class DashboardService {
  static async getTopEmployees({ month, year, limit = 3 }) {
    try {
      const res = await ApiClient.get(ApiUrl.ADMIN_DASHBOARD_TOP_EMPLOYEES, {
        params: { month, year, limit },
      });
      return res.data;
    } catch (err) {
      console.error("Error fetching top employees:", err);
      throw err;
    }
  }
}

export default DashboardService;
