import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class EmployeeService {
  static async checkIn() {
    try {
      const response = await ApiClient.post(ApiUrl.EMPLOYEE_CHECKIN);
      return response.data;
    } catch (error) {
      console.error('Error during check-in:', error);
      throw error;
    }
  }

  static async checkOut() {
    try {
      const response = await ApiClient.post(ApiUrl.EMPLOYEE_CHECKOUT);
      return response.data;
    } catch (error) {
      console.error('Error during check-out:', error);
      throw error;
    }
  }

  static async getEmployees(page = 0, size = 20) {
    try {
      const response = await ApiClient.get(ApiUrl.ADMIN_EMPLOYEES, {
        params: { page, size },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching employees:', error);
      throw error;
    }
  }

  static async createEmployee(employeeData) {
    try {
      const response = await ApiClient.post(
        ApiUrl.ADMIN_EMPLOYEES,
        employeeData,
      );
      return response.data;
    } catch (error) {
      console.error('Error creating employee:', error);
      throw error;
    }
  }

  static async updateEmployee(id, updates) {
    try {
      const response = await ApiClient.put(
        `${ApiUrl.ADMIN_EMPLOYEES}/${id}`,
        updates,
      );
      return response.data;
    } catch (error) {
      console.error('Error updating employee:', error);
      throw error;
    }
  }

  static async getWorkDetail(id, month, year) {
    try {
      const response = await ApiClient.get(
        `${ApiUrl.ADMIN_EMPLOYEES}/${id}`,
        { params: { month, year } },
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching employee work detail:', error);
      throw error;
    }
  }

  static async deleteEmployee(id) {
    try {
      const response = await ApiClient.delete(
        `${ApiUrl.ADMIN_EMPLOYEES}/${id}`,
      );
      return response.data;
    } catch (error) {
      console.error('Error deleting employee:', error);
      throw error;
    }
  }
}

export default EmployeeService;
