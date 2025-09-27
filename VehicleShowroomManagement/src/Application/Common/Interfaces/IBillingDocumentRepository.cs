using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IBillingDocumentRepository : IRepository<BillingDocument>
    {
        Task<IEnumerable<BillingDocument>> GetBySalesOrderIdAsync(string salesOrderId);
        Task<IEnumerable<BillingDocument>> GetByEmployeeIdAsync(string employeeId);
        Task<BillingDocument?> GetByBillNumberAsync(string billNumber);
        Task<IEnumerable<BillingDocument>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}