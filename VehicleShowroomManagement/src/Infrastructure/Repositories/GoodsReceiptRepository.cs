using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class GoodsReceiptRepository : MongoRepository<GoodsReceipt>, IGoodsReceiptRepository
    {
        public GoodsReceiptRepository(VehicleShowroomDbContext context) : base(context, "goodsreceipts")
        {
        }

        public async Task<IEnumerable<GoodsReceipt>> GetByPurchaseOrderIdAsync(string purchaseOrderId)
        {
            var filter = Builders<GoodsReceipt>.Filter.Eq(g => g.PurchaseOrderId, purchaseOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<GoodsReceipt>> GetByReceiptNumberAsync(string receiptNumber)
        {
            var filter = Builders<GoodsReceipt>.Filter.Eq(g => g.ReceiptNumber, receiptNumber);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<GoodsReceipt?> GetByVehicleIdAsync(string vehicleId)
        {
            var filter = Builders<GoodsReceipt>.Filter.Eq(g => g.VehicleId, vehicleId);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}