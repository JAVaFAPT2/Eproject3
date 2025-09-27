using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IGoodsReceiptRepository : IRepository<GoodsReceipt>
    {
        Task<IEnumerable<GoodsReceipt>> GetByPurchaseOrderIdAsync(string purchaseOrderId);
        Task<IEnumerable<GoodsReceipt>> GetByReceiptNumberAsync(string receiptNumber);
        Task<GoodsReceipt?> GetByVehicleIdAsync(string vehicleId);
    }
}