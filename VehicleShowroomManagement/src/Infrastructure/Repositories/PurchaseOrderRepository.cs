using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class PurchaseOrderRepository : MongoRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(VehicleShowroomDbContext context) : base(context, "purchaseorders")
        {
        }

        public async Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(string supplierId)
        {
            var filter = Builders<PurchaseOrder>.Filter.Eq(p => p.SupplierId, supplierId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status)
        {
            var filter = Builders<PurchaseOrder>.Filter.Eq(p => p.Status, status);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber)
        {
            var filter = Builders<PurchaseOrder>.Filter.Eq(p => p.OrderNumber, orderNumber);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}