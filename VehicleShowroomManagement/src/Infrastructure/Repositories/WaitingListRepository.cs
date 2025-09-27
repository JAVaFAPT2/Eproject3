using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class WaitingListRepository : MongoRepository<WaitingList>, IWaitingListRepository
    {
        public WaitingListRepository(VehicleShowroomDbContext context) : base(context, "waitinglists")
        {
        }

        public async Task<IEnumerable<WaitingList>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<WaitingList>.Filter.Eq(w => w.CustomerId, customerId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<WaitingList>> GetByModelNumberAsync(string modelNumber)
        {
            var filter = Builders<WaitingList>.Filter.Eq(w => w.ModelNumber, modelNumber);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<WaitingList>> GetByStatusAsync(string status)
        {
            var filter = Builders<WaitingList>.Filter.Eq(w => w.Status, status);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<WaitingList?> GetByCustomerAndModelAsync(string customerId, string modelNumber)
        {
            var filter = Builders<WaitingList>.Filter.And(
                Builders<WaitingList>.Filter.Eq(w => w.CustomerId, customerId),
                Builders<WaitingList>.Filter.Eq(w => w.ModelNumber, modelNumber)
            );
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}