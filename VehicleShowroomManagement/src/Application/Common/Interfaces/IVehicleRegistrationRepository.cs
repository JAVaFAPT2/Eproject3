using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IVehicleRegistrationRepository : IRepository<VehicleRegistration>
    {
        Task<VehicleRegistration?> GetByRegistrationNumberAsync(string registrationNumber);
        Task<VehicleRegistration?> GetByVinAsync(string vin);
        Task<VehicleRegistration?> GetByVehicleIdAsync(string vehicleId);
        Task<IEnumerable<VehicleRegistration>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<VehicleRegistration>> GetExpiringSoonAsync(DateTime beforeDate);
    }
}