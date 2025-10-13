import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleModelService = {
  create(payload) {
    return ApiClient.post(ApiUrl.VEHICLE_MODELS.BASE, payload).then(
      (r) => r.data,
    );
  },
  update(modelNumber, payload) {
    return ApiClient.put(
      ApiUrl.VEHICLE_MODELS.BY_MODEL(modelNumber),
      payload,
    ).then((r) => r.data);
  },
  getAll(params) {
    return ApiClient.get(ApiUrl.VEHICLE_MODELS.BASE, { params }).then(
      (r) => r.data,
    );
  },
};

export default VehicleModelService;
