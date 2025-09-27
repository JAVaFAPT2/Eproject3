using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class ServiceOrderRepository : MongoRepository<ServiceOrder>, IServiceOrderRepository
    {
        public ServiceOrderRepository(VehicleShowroomDbContext context) : base(context, "serviceorders")
        {
        }

        public async Task<IEnumerable<ServiceOrder>> GetBySalesOrderIdAsync(string salesOrderId)
        {
            var filter = Builders<ServiceOrder>.Filter.Eq(s => s.SalesOrderId, salesOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<ServiceOrder>> GetByEmployeeIdAsync(string employeeId)
        {
            var filter = Builders<ServiceOrder>.Filter.Eq(s => s.EmployeeId, employeeId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<ServiceOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<ServiceOrder>.Filter.And(
                Builders<ServiceOrder>.Filter.Gte(s => s.ServiceDate, startDate),
                Builders<ServiceOrder>.Filter.Lte(s => s.ServiceDate, endDate)
            );
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<ServiceOrder?> GetByServiceOrderNumberAsync(string serviceOrderNumber)
        {
            var filter = Builders<ServiceOrder>.Filter.Eq(s => s.ServiceOrderNumber, serviceOrderNumber);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}