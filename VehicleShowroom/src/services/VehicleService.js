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
    const res = await ApiClient.put(`${ApiUrl.VEHICLES.BASE}/${id}`, data);
    return res.data;
  },

  // 🗑️ Delete single
  delete: async (id) => {
    const res = await ApiClient.delete(`${ApiUrl.VEHICLES.BASE}/${id}`);
    return res.data;
  },

  // 🧹 Bulk delete
  bulkDelete: async (vehicleIds) => {
    const res = await ApiClient.post(ApiUrl.VEHICLES.BULK_DELETE, {
      vehicleIds,
    });
    return res.data;
  },

  // 🚘 Update status
  updateStatus: async (id, status) => {
    const res = await ApiClient.put(`${ApiUrl.VEHICLES.BASE}/${id}/status`, {
      status,
    });
    return res.data;
  },
};

export default VehicleService;
