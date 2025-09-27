using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class PurchaseOrderLineRepository : MongoRepository<PurchaseOrderLine>, IPurchaseOrderLineRepository
    {
        public PurchaseOrderLineRepository(VehicleShowroomDbContext context) : base(context, "purchaseorderlines")
        {
        }

        public async Task<IEnumerable<PurchaseOrderLine>> GetByPurchaseOrderIdAsync(string purchaseOrderId)
        {
            var filter = Builders<PurchaseOrderLine>.Filter.Eq(p => p.PurchaseOrderId, purchaseOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrderLine>> GetByModelNumberAsync(string modelNumber)
        {
            var filter = Builders<PurchaseOrderLine>.Filter.Eq(p => p.ModelNumber, modelNumber);
            return await Collection.Find(filter).ToListAsync();
        }
    }
}