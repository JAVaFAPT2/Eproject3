import { vehicleModels } from '../mockData/vehicleModels.js';
import { vehiclePhotos } from '../mockData/vehiclePhotos.js';
import { simulateDelay } from './utils.js';

/**
 * Gắn ảnh (photos) tương ứng cho mỗi vehicle model
 */
const attachPhotosToModel = (model) => {
  const photos = vehiclePhotos.filter(
    (p) => p.vehicleModelId === model.modelNumber,
  );
  return { ...model, photos };
};

const VehicleModelService = {
  /**
   * ✅ Lấy tất cả model kèm ảnh
   */
  getAll: () => {
    const result = vehicleModels.map(attachPhotosToModel);
    return simulateDelay(result);
  },

  /**
   * ✅ Lấy 1 model theo id (kèm ảnh)
   */
  getById: (id) => {
    const model = vehicleModels.find((x) => x.modelNumber === id);
    if (!model) return simulateDelay(null);
    return simulateDelay(attachPhotosToModel(model));
  },

  /**
   * ✅ Tạo mới model
   */
  create: (data) => {
    vehicleModels.push(data);
    return simulateDelay({ message: 'Vehicle model created successfully' });
  },

  /**
   * ✅ Cập nhật model
   */
  update: (id, data) => {
    const i = vehicleModels.findIndex((x) => x.modelNumber === id);
    if (i >= 0) vehicleModels[i] = { ...vehicleModels[i], ...data };
    return simulateDelay({ message: 'Vehicle model updated successfully' });
  },

  /**
   * ✅ Xóa model
   */
  delete: (id) => {
    const i = vehicleModels.findIndex((x) => x.modelNumber === id);
    if (i >= 0) vehicleModels.splice(i, 1);
    return simulateDelay({ message: 'Vehicle model deleted successfully' });
  },
};

export default VehicleModelService;
