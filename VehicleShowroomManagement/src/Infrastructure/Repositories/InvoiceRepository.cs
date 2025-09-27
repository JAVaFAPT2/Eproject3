using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class InvoiceRepository : MongoRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(VehicleShowroomDbContext context) : base(context, "invoices")
        {
        }

        public async Task<IEnumerable<Invoice>> GetBySalesOrderIdAsync(string salesOrderId)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.SalesOrderId, salesOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.InvoiceNumber, invoiceNumber);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.CustomerId, customerId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByStatusAsync(string status)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.Status, status);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Invoice>.Filter.And(
                Builders<Invoice>.Filter.Gte(i => i.InvoiceDate, startDate),
                Builders<Invoice>.Filter.Lte(i => i.InvoiceDate, endDate)
            );
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetOverdueAsync(DateTime currentDate)
        {
            var filter = Builders<Invoice>.Filter.And(
                Builders<Invoice>.Filter.Eq(i => i.Status, "Pending"),
                Builders<Invoice>.Filter.Lt(i => i.DueDate, currentDate)
            );
            return await Collection.Find(filter).ToListAsync();
        }
    }
}