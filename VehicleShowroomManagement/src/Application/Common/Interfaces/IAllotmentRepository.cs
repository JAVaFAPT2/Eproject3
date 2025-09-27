using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IAllotmentRepository : IRepository<Allotment>
    {
        Task<IEnumerable<Allotment>> GetByVehicleIdAsync(string vehicleId);
        Task<IEnumerable<Allotment>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Allotment>> GetByEmployeeIdAsync(string employeeId);
        Task<Allotment?> GetByVehicleAndCustomerAsync(string vehicleId, string customerId);
    }
}