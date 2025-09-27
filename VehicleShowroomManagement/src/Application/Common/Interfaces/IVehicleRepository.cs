using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Repository interface for Vehicle aggregate
    /// </summary>
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(string id);
        Task<Vehicle?> GetByVehicleIdAsync(string vehicleId);
        Task<Vehicle?> GetByVinAsync(string vin);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status);
        Task<IEnumerable<Vehicle>> GetByModelNumberAsync(string modelNumber);
        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
        Task<List<Vehicle>> GetVehiclesAsync(int pageNumber, int pageSize, VehicleStatus? status, string? brand);
        Task<int> GetVehiclesCountAsync(VehicleStatus? status, string? brand);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> VehicleIdExistsAsync(string vehicleId);
        Task<bool> VinExistsAsync(string vin);
    }
}