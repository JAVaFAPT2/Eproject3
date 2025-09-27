using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class VehicleImageRepository : MongoRepository<VehicleImage>, IVehicleImageRepository
    {
        public VehicleImageRepository(VehicleShowroomDbContext context) : base(context, "vehicleimages")
        {
        }

        public async Task<IEnumerable<VehicleImage>> GetByVehicleIdAsync(string vehicleId)
        {
            var filter = Builders<VehicleImage>.Filter.Eq(v => v.VehicleId, vehicleId);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<VehicleImage?> GetPrimaryImageByVehicleIdAsync(string vehicleId)
        {
            var filter = Builders<VehicleImage>.Filter.And(
                Builders<VehicleImage>.Filter.Eq(v => v.VehicleId, vehicleId),
                Builders<VehicleImage>.Filter.Eq(v => v.IsPrimary, true)
            );
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VehicleImage>> GetByImageTypeAsync(string imageType)
        {
            var filter = Builders<VehicleImage>.Filter.Eq(v => v.ImageType, imageType);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<VehicleImage>> GetByFileNameAsync(string fileName)
        {
            var filter = Builders<VehicleImage>.Filter.Eq(v => v.FileName, fileName);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<int> GetImageCountByVehicleAsync(string vehicleId)
        {
            var filter = Builders<VehicleImage>.Filter.Eq(v => v.VehicleId, vehicleId);
            return (int)await Collection.CountDocumentsAsync(filter);
        }
    }
}