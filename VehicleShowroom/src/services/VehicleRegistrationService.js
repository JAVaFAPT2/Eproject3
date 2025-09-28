import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const VehicleRegistrationService = {
  getAll: (params) => ApiClient.get(ApiUrl.VEHICLE_REGISTRATIONS.BASE, { params }),
  create: (data) => ApiClient.post(ApiUrl.VEHICLE_REGISTRATIONS.BASE, data),
};

export default VehicleRegistrationService;
