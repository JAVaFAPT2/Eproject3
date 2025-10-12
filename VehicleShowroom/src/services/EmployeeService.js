import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

const EmployeeService = {
  getAll: (params) => ApiClient.get(ApiUrl.EMPLOYEES.BASE, { params }),
  getById: (id) => ApiClient.get(ApiUrl.EMPLOYEES.BY_ID(id)),
  create: (data) => ApiClient.post(ApiUrl.EMPLOYEES.BASE, data),
  update: (id, data) => ApiClient.put(ApiUrl.EMPLOYEES.BY_ID(id), data),
  delete: (id) => ApiClient.delete(ApiUrl.EMPLOYEES.BY_ID(id)),
  getProfile: () => ApiClient.get(ApiUrl.EMPLOYEES.PROFILE),
};

export default EmployeeService;
