using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IPurchaseOrderLineRepository : IRepository<PurchaseOrderLine>
    {
        Task<IEnumerable<PurchaseOrderLine>> GetByPurchaseOrderIdAsync(string purchaseOrderId);
        Task<IEnumerable<PurchaseOrderLine>> GetByModelNumberAsync(string modelNumber);
    }
}