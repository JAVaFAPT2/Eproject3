using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(string supplierId);
        Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status);
        Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber);
    }
}