using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class BillingDocumentRepository : MongoRepository<BillingDocument>, IBillingDocumentRepository
    {
        public BillingDocumentRepository(VehicleShowroomDbContext context) : base(context, "billingdocuments")
        {
        }

        public async Task<IEnumerable<BillingDocument>> GetBySalesOrderIdAsync(string salesOrderId)
        {
            var filter = Builders<BillingDocument>.Filter.Eq(b => b.SalesOrderId, salesOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<BillingDocument>> GetByEmployeeIdAsync(string employeeId)
        {
            var filter = Builders<BillingDocument>.Filter.Eq(b => b.EmployeeId, employeeId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<BillingDocument?> GetByBillNumberAsync(string billNumber)
        {
            var filter = Builders<BillingDocument>.Filter.Eq(b => b.BillNumber, billNumber);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BillingDocument>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<BillingDocument>.Filter.And(
                Builders<BillingDocument>.Filter.Gte(b => b.BillDate, startDate),
                Builders<BillingDocument>.Filter.Lte(b => b.BillDate, endDate)
            );
            return await Collection.Find(filter).ToListAsync();
        }
    }
}