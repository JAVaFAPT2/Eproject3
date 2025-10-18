import ApiClient, { uploadClient } from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const VehiclePhotoService = {
  // 🟢 Get all photos by model number
  getByModelNumber: async (modelNumber) => {
    if (!modelNumber) throw new Error('Model number is required');
    const res = await ApiClient.get(
      ApiUrl.VEHICLE_PHOTOS.BY_MODEL(modelNumber),
    );
    return res.data;
  },

  // 🟣 Upload multiple photos (multipart/form-data)
  upload: async (modelNumber, files) => {
    if (!modelNumber) throw new Error('Model number is required');
    if (!files || files.length === 0) throw new Error('No files provided');

    const formData = new FormData();
    files.forEach((file) => formData.append('files', file));

    const res = await uploadClient.post(
      ApiUrl.VEHICLE_PHOTOS.UPLOAD(modelNumber),
      formData,
    );
    return res.data;
  },

  // 🔍 Get photo detail
  getById: async (photoId) => {
    if (!photoId) throw new Error('Photo ID is required');
    const res = await ApiClient.get(ApiUrl.VEHICLE_PHOTOS.BY_ID(photoId));
    return res.data;
  },

  // ✏️ Update photo info
  update: async (photoId, data) => {
    if (!photoId) throw new Error('Photo ID is required');
    const res = await ApiClient.put(ApiUrl.VEHICLE_PHOTOS.BY_ID(photoId), data);
    return res.data;
  },

  // 🗑️ Delete a photo
  delete: async (photoId) => {
    if (!photoId) throw new Error('Photo ID is required');
    const res = await ApiClient.delete(ApiUrl.VEHICLE_PHOTOS.BY_ID(photoId));
    return res.data;
  },
};

export default VehiclePhotoService;
