import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const UserService = {
  // GET /api/users?searchTerm&roleId&pageNumber&pageSize
  getAll(params) {
    return ApiClient.get(ApiUrl.USERS.BASE, { params }).then((r) => r.data);
  },
  getById(id) {
    return ApiClient.get(ApiUrl.USERS.BY_ID(id)).then((r) => r.data);
  },
  create(payload) {
    return ApiClient.post(ApiUrl.USERS.BASE, payload).then((r) => r.data);
  },
  update(id, payload) {
    return ApiClient.put(ApiUrl.USERS.BY_ID(id), payload).then((r) => r.data);
  },
  remove(id) {
    return ApiClient.delete(ApiUrl.USERS.BY_ID(id)).then((r) => r.data);
  },
};

export default UserService;
