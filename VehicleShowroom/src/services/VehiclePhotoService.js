import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehiclePhotoService = {
  /**
   * 🔹 Get all photos of a specific vehicle model
   * GET /api/vehicle-models/{modelNumber}/photos
   */
  getByModelNumber: async (modelNumber) => {
    if (!modelNumber) throw new Error('Model number is required');
    const res = await ApiClient.get(
      ApiUrl.VEHICLE_PHOTOS.BY_MODEL(modelNumber),
    );
    return res.data;
  },
};

export default VehiclePhotoService;
