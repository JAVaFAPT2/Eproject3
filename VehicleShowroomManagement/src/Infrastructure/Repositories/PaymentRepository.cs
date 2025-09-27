using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class PaymentRepository : MongoRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(VehicleShowroomDbContext context) : base(context, "payments")
        {
        }

        public async Task<IEnumerable<Payment>> GetByInvoiceIdAsync(string invoiceId)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.InvoiceId, invoiceId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetBySalesOrderIdAsync(string salesOrderId)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.SalesOrderId, salesOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPaymentMethodAsync(string paymentMethod)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.PaymentMethod, paymentMethod);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.Status, status);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Payment>.Filter.And(
                Builders<Payment>.Filter.Gte(p => p.PaymentDate, startDate),
                Builders<Payment>.Filter.Lte(p => p.PaymentDate, endDate)
            );
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<decimal> GetTotalPaymentsByInvoiceAsync(string invoiceId)
        {
            var filter = Builders<Payment>.Filter.And(
                Builders<Payment>.Filter.Eq(p => p.InvoiceId, invoiceId),
                Builders<Payment>.Filter.Eq(p => p.Status, "Completed")
            );

            var payments = await Collection.Find(filter).ToListAsync();
            return payments.Sum(p => p.Amount);
        }
    }
}