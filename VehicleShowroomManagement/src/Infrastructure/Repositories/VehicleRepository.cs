using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB implementation of Vehicle repository
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public VehicleRepository(IMongoDatabase database)
        {
            _vehicles = database.GetCollection<Vehicle>("vehicles");
        }

        public async Task<Vehicle?> GetByIdAsync(string id)
        {
            return await _vehicles.Find(v => v.Id == id && !v.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Vehicle?> GetByVehicleIdAsync(string vehicleId)
        {
            return await _vehicles.Find(v => v.VehicleId == vehicleId && !v.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Vehicle?> GetByVinAsync(string vin)
        {
            return await _vehicles.Find(v => v.Vin == vin && !v.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _vehicles.Find(v => !v.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status)
        {
            return await _vehicles.Find(v => v.Status == status && !v.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetByModelNumberAsync(string modelNumber)
        {
            return await _vehicles.Find(v => v.ModelNumber == modelNumber && !v.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _vehicles.Find(v => v.Status == VehicleStatus.Available && !v.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            await _vehicles.ReplaceOneAsync(v => v.Id == vehicle.Id, vehicle);
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<Vehicle>.Update
                .Set(v => v.IsDeleted, true)
                .Set(v => v.DeletedAt, DateTime.UtcNow);
            
            await _vehicles.UpdateOneAsync(v => v.Id == id, update);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var count = await _vehicles.CountDocumentsAsync(v => v.Id == id && !v.IsDeleted);
            return count > 0;
        }

        public async Task<bool> VehicleIdExistsAsync(string vehicleId)
        {
            var count = await _vehicles.CountDocumentsAsync(v => v.VehicleId == vehicleId && !v.IsDeleted);
            return count > 0;
        }

        public async Task<bool> VinExistsAsync(string vin)
        {
            var count = await _vehicles.CountDocumentsAsync(v => v.Vin == vin && !v.IsDeleted);
            return count > 0;
        }
    }
}