using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class VehicleRegistrationRepository : MongoRepository<VehicleRegistration>, IVehicleRegistrationRepository
    {
        public VehicleRegistrationRepository(VehicleShowroomDbContext context) : base(context, "vehicleregistrations")
        {
        }

        public async Task<VehicleRegistration?> GetByRegistrationNumberAsync(string registrationNumber)
        {
            var filter = Builders<VehicleRegistration>.Filter.Eq(v => v.RegistrationNumber, registrationNumber);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<VehicleRegistration?> GetByVinAsync(string vin)
        {
            var filter = Builders<VehicleRegistration>.Filter.Eq(v => v.Vin, vin);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<VehicleRegistration?> GetByVehicleIdAsync(string vehicleId)
        {
            var filter = Builders<VehicleRegistration>.Filter.Eq(v => v.VehicleId, vehicleId);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VehicleRegistration>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<VehicleRegistration>.Filter.Eq(v => v.CustomerId, customerId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<VehicleRegistration>> GetExpiringSoonAsync(DateTime beforeDate)
        {
            var filter = Builders<VehicleRegistration>.Filter.Lt(v => v.ExpiryDate, beforeDate);
            return await Collection.Find(filter).ToListAsync();
        }
    }
}