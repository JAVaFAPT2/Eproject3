using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IVehicleImageRepository : IRepository<VehicleImage>
    {
        Task<IEnumerable<VehicleImage>> GetByVehicleIdAsync(string vehicleId);
        Task<VehicleImage?> GetPrimaryImageByVehicleIdAsync(string vehicleId);
        Task<IEnumerable<VehicleImage>> GetByImageTypeAsync(string imageType);
        Task<IEnumerable<VehicleImage>> GetByFileNameAsync(string fileName);
        Task<int> GetImageCountByVehicleAsync(string vehicleId);
    }
}