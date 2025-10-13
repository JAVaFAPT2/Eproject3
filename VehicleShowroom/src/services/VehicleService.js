import { vehicles } from '../mockData/vehicles.js';
import { vehicleSpecs } from '../mockData/vehicleSpecs.js';
import { vehiclePhotos } from '../mockData/vehiclePhotos.js';
import { simulateDelay } from './utils.js';

const VehicleService = {
  getAll: (filter = {}) => {
    let data = vehicles.map((v) => {
      const specs = vehicleSpecs.filter((s) => s.vehicleId === v.vehicleId);
      const photos = vehiclePhotos.filter(
        (p) =>
          p.vehicleId === v.vehicleId ||
          p.vehicleModelId?.toLowerCase() ===
            v.modelNumber?.replace(/\d+/g, '').toLowerCase(),
      );

      return {
        ...v,
        specs,
        photos,
      };
    });

    // ✅ áp dụng bộ lọc cơ bản
    if (filter.status) data = data.filter((v) => v.status === filter.status);

    if (filter.modelNumber)
      data = data.filter((v) => v.modelNumber === filter.modelNumber);

    return simulateDelay(data);
  },

  getById: (id) => {
    const v = vehicles.find((x) => x.vehicleId === id);
    if (!v) return simulateDelay(null);

    const specs = vehicleSpecs.filter((s) => s.vehicleId === v.vehicleId);
    const photos = vehiclePhotos.filter(
      (p) =>
        p.vehicleId === v.vehicleId ||
        p.vehicleModelId?.toLowerCase() ===
          v.modelNumber?.replace(/\d+/g, '').toLowerCase(),
    );

    return simulateDelay({ ...v, specs, photos });
  },

  create: (data) => {
    vehicles.push(data);
    return simulateDelay({ message: 'Vehicle created successfully' });
  },

  update: (id, data) => {
    const i = vehicles.findIndex((v) => v.vehicleId === id);
    if (i >= 0) vehicles[i] = { ...vehicles[i], ...data };
    return simulateDelay({ message: 'Vehicle updated successfully' });
  },

  updateStatus: (id, status) => {
    const v = vehicles.find((x) => x.vehicleId === id);
    if (v) v.status = status;
    return simulateDelay({ message: 'Vehicle status updated successfully' });
  },

  delete: (id) => {
    const i = vehicles.findIndex((v) => v.vehicleId === id);
    if (i >= 0) vehicles.splice(i, 1);
    return simulateDelay({ message: 'Vehicle deleted successfully' });
  },
};

export default VehicleService;
