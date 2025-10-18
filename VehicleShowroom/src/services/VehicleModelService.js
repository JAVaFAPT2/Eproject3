import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleModelService = {
  /**
   * 🟢 Tạo Vehicle Model (multipart/form-data)
   * POST /api/VehicleModels
   */
  create: async (modelData) => {
    const res = await ApiClient.post(ApiUrl.VEHICLE_MODELS.BASE, modelData);
    return res.data;
  },

  /**
   * 🟡 Cập nhật Vehicle Model (multipart/form-data)
   * PUT /api/VehicleModels/{modelNumber}
   */
  update: async (modelNumber, modelData) => {
    const res = await ApiClient.put(
      ApiUrl.VEHICLE_MODELS.BY_ID(modelNumber),
      modelData,
    );
    return res.data;
  },

  delete: async (modelNumber) => {
    const res = await ApiClient.delete(
      ApiUrl.VEHICLE_MODELS.BY_ID(modelNumber),
    );
    return res.data;
  },

  get: async (params = {}) => {
    const res = await ApiClient.get(ApiUrl.VEHICLE_MODELS.BASE, { params });
    return res.data;
  },

  /**
   * 🔹 Lấy model theo slug
   * GET /api/VehicleModels/slug/{slug}
   */
  getBySlug: async (slug) => {
    const res = await ApiClient.get(ApiUrl.VEHICLE_MODELS.BY_SLUG(slug));
    return res.data;
  },
};

export default VehicleModelService;
