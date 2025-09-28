import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const ImageService = {
  uploadVehicleImage: (id, file) => {
    const formData = new FormData();
    formData.append('file', file);
    return ApiClient.post(ApiUrl.IMAGES.UPLOAD_VEHICLE(id), formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },
};

export default ImageService;
