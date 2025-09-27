using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class AllotmentRepository : MongoRepository<Allotment>, IAllotmentRepository
    {
        public AllotmentRepository(VehicleShowroomDbContext context) : base(context, "allotments")
        {
        }

        public async Task<IEnumerable<Allotment>> GetByVehicleIdAsync(string vehicleId)
        {
            var filter = Builders<Allotment>.Filter.Eq(a => a.VehicleId, vehicleId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Allotment>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Allotment>.Filter.Eq(a => a.CustomerId, customerId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Allotment>> GetByEmployeeIdAsync(string employeeId)
        {
            var filter = Builders<Allotment>.Filter.Eq(a => a.EmployeeId, employeeId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<Allotment?> GetByVehicleAndCustomerAsync(string vehicleId, string customerId)
        {
            var filter = Builders<Allotment>.Filter.And(
                Builders<Allotment>.Filter.Eq(a => a.VehicleId, vehicleId),
                Builders<Allotment>.Filter.Eq(a => a.CustomerId, customerId)
            );
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}