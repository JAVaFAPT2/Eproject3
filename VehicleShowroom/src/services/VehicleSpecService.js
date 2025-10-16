import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleSpecService = {
  getByModelNumber: async (modelNumber) => {
    const res = await ApiClient.get(ApiUrl.VEHICLE_SPECS.BY_MODEL(modelNumber));
    return res.data;
  },
  create: async (modelNumber, data) => {
    const res = await ApiClient.post(
      ApiUrl.VEHICLE_SPECS.BY_MODEL(modelNumber),
      data,
    );
    return res.data;
  },
};

export default VehicleSpecService;
