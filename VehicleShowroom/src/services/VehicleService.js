import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleService = {
  // 🟢 Create new vehicle
  create: async (data) => {
    const res = await ApiClient.post(ApiUrl.VEHICLES.BASE, data);
    return res.data;
  },

  get: async (params = {}) => {
    const res = await ApiClient.get(ApiUrl.VEHICLES.BASE, { params });
    return res.data;
  },

  // ✏️ Update vehicle
  update: async (id, data) => {
    const res = await ApiClient.put(ApiUrl.VEHICLES.BY_ID(id), data);
    return res.data;
  },

  // 🗑️ Delete single
  delete: async (id) => {
    const res = await ApiClient.delete(ApiUrl.VEHICLES.BY_ID(id));
    return res.data;
  },

  // 🚘 Update status
  updateStatus: async (id, status) => {
    const res = await ApiClient.put(ApiUrl.VEHICLES.STATUS(id), { status });
    return res.data;
  },

  updateLicensePlate: async (id, licensePlate) => {
    const res = await ApiClient.put(ApiUrl.VEHICLES.LICENSE_PLATE(id), {
      licensePlate,
    });
    return res.data;
  },
};

export default VehicleService;
