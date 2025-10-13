import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleService = {
  create(payload) {
    return ApiClient.post(ApiUrl.VEHICLES.BASE, payload).then((r) => r.data);
  },
  getById(id) {
    return ApiClient.get(ApiUrl.VEHICLES.BY_ID(id)).then((r) => r.data);
  },
  getAll(params) {
    return ApiClient.get(ApiUrl.VEHICLES.BASE, { params }).then((r) => r.data);
  },
  search(params) {
    return ApiClient.get(ApiUrl.VEHICLES.SEARCH, { params }).then(
      (r) => r.data,
    );
  },
  update(id, payload) {
    return ApiClient.put(ApiUrl.VEHICLES.BY_ID(id), payload).then(
      (r) => r.data,
    );
  },
  updateStatus(id, { status }) {
    return ApiClient.put(ApiUrl.VEHICLES.STATUS(id), { status }).then(
      (r) => r.data,
    );
  },
  remove(id) {
    return ApiClient.delete(ApiUrl.VEHICLES.BY_ID(id)).then((r) => r.data);
  },
  bulkDelete(vehicleIds) {
    return ApiClient.post(ApiUrl.VEHICLES.BULK_DELETE, { vehicleIds }).then(
      (r) => r.data,
    );
  },
};

export default VehicleService;
