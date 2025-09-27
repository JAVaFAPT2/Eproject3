using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IReturnRequestRepository : IRepository<ReturnRequest>
    {
        Task<IEnumerable<ReturnRequest>> GetBySalesOrderIdAsync(string salesOrderId);
        Task<IEnumerable<ReturnRequest>> GetByVehicleIdAsync(string vehicleId);
        Task<IEnumerable<ReturnRequest>> GetByStatusAsync(string status);
        Task<IEnumerable<ReturnRequest>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}