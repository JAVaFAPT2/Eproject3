import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleSpecService = {
  // 🔹 Get all specs by model number
  getByModelNumber: async (modelNumber) => {
    if (!modelNumber) throw new Error('Model number is required');
    const res = await ApiClient.get(ApiUrl.VEHICLE_SPECS.BY_MODEL(modelNumber));
    return res.data;
  },

  // 🔍 Get spec detail by ID
  getById: async (specId) => {
    if (!specId) throw new Error('Spec ID is required');
    const res = await ApiClient.get(ApiUrl.VEHICLE_SPECS.BY_ID(specId));
    return res.data;
  },

  // 🟢 Create a new spec for a model
  create: async (modelNumber, data) => {
    if (!modelNumber) throw new Error('Model number is required');
    const res = await ApiClient.post(
      ApiUrl.VEHICLE_SPECS.BY_MODEL(modelNumber),
      data,
    );
    return res.data;
  },

  // ✏️ Update existing spec
  update: async (specId, data) => {
    if (!specId) throw new Error('Spec ID is required');
    const res = await ApiClient.put(ApiUrl.VEHICLE_SPECS.BY_ID(specId), data);
    return res.data;
  },

  // 🗑️ Delete a spec
  delete: async (specId) => {
    if (!specId) throw new Error('Spec ID is required');
    const res = await ApiClient.delete(ApiUrl.VEHICLE_SPECS.BY_ID(specId));
    return res.data;
  },
};

export default VehicleSpecService;
