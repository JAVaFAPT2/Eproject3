using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IServiceOrderRepository : IRepository<ServiceOrder>
    {
        Task<IEnumerable<ServiceOrder>> GetBySalesOrderIdAsync(string salesOrderId);
        Task<IEnumerable<ServiceOrder>> GetByEmployeeIdAsync(string employeeId);
        Task<IEnumerable<ServiceOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ServiceOrder?> GetByServiceOrderNumberAsync(string serviceOrderNumber);
    }
}