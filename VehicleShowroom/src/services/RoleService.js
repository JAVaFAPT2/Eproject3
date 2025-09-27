import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class RoleService {
  static async getRoles() {
    try {
      const response = await ApiClient.get(ApiUrl.ADMIN_ROLES);
      return response.data;
    } catch (error) {
      console.error('Error fetching roles:', error);
      throw error;
    }
  }

  static async getRoleStats() {
    try {
       const response = await ApiClient.get(`${ApiUrl.ADMIN_ROLES}/stats`);
      return response.data;
    } catch (error) {
      console.error('Error fetching roles:', error);
      throw error;
    }
  }
}

export default RoleService;
