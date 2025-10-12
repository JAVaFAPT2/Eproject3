import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

const VehicleService = {
  getAll: (params) => ApiClient.get(ApiUrl.VEHICLES.BASE, { params }),
  getById: (id) => ApiClient.get(ApiUrl.VEHICLES.BY_ID(id)),
  create: (data) => ApiClient.post(ApiUrl.VEHICLES.BASE, data),
  update: (id, data) => ApiClient.put(ApiUrl.VEHICLES.BY_ID(id), data),
  delete: (id) => ApiClient.post(ApiUrl.VEHICLES.BY_ID(id)),
  deleteMany: (ids) => ApiClient.post(ApiUrl.VEHICLES.DELETE_MANY, { ids }),
};

export default VehicleService;
