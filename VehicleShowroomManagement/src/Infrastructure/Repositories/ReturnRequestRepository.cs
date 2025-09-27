using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class ReturnRequestRepository : MongoRepository<ReturnRequest>, IReturnRequestRepository
    {
        public ReturnRequestRepository(VehicleShowroomDbContext context) : base(context, "returnrequests")
        {
        }

        public async Task<IEnumerable<ReturnRequest>> GetBySalesOrderIdAsync(string salesOrderId)
        {
            var filter = Builders<ReturnRequest>.Filter.Eq(r => r.SalesOrderId, salesOrderId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<ReturnRequest>> GetByVehicleIdAsync(string vehicleId)
        {
            var filter = Builders<ReturnRequest>.Filter.Eq(r => r.VehicleId, vehicleId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<ReturnRequest>> GetByStatusAsync(string status)
        {
            var filter = Builders<ReturnRequest>.Filter.Eq(r => r.Status, status);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<ReturnRequest>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<ReturnRequest>.Filter.And(
                Builders<ReturnRequest>.Filter.Gte(r => r.RequestDate, startDate),
                Builders<ReturnRequest>.Filter.Lte(r => r.RequestDate, endDate)
            );
            return await Collection.Find(filter).ToListAsync();
        }
    }
}