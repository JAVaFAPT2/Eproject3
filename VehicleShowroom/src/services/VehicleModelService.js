import ApiClient, { uploadClient } from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehicleModelService = {
  /**
   * 🟢 Tạo Vehicle Model (multipart/form-data)
   * POST /api/VehicleModels
   */
  create: async (formData) => {
    const res = await uploadClient.post(ApiUrl.VEHICLE_MODELS.BASE, formData);
    return res.data;
  },

  /**
   * 🟡 Cập nhật Vehicle Model (multipart/form-data)
   * PUT /api/VehicleModels/{modelNumber}
   */
  update: async (modelNumber, data) => {
    const formData = new FormData();

    formData.append('modelNumber', modelNumber);
    formData.append('name', data.name);
    formData.append('price', data.price ? Number(data.price) : 0);
    formData.append('description', data.description || '');
    formData.append('parentId', data.parentId || '');
    formData.append('level', data.level || 1);
    formData.append('slug', data.slug || '');

    if (Array.isArray(data.files)) {
      data.files.forEach((file) => formData.append('files', file));
    } else if (data.files) {
      formData.append('files', data.files);
    }

    const res = await uploadClient.put(
      `${ApiUrl.VEHICLE_MODELS.BASE}/${modelNumber}`,
      formData,
    );
    return res.data;
  },

  /**
   * 🔍 Tìm kiếm Vehicle Model
   * GET /api/VehicleModels/search?parentModelNumber=&seats=&fuelType=&pageNumber=&pageSize=
   */
  search: async (params = {}) => {
    const res = await ApiClient.get(ApiUrl.VEHICLE_MODELS.SEARCH, { params });
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
    const res = await ApiClient.get(`${ApiUrl.VEHICLE_MODELS.SLUG}/${slug}`);
    return res.data;
  },
};

export default VehicleModelService;
