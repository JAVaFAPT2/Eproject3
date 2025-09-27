using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    public class SupplierRepository : MongoRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(VehicleShowroomDbContext context) : base(context, "suppliers")
        {
        }

        public async Task<Supplier?> GetByCompanyNameAsync(string companyName)
        {
            var filter = Builders<Supplier>.Filter.Eq(s => s.CompanyName, companyName);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Supplier>> GetByCountryAsync(string country)
        {
            var filter = Builders<Supplier>.Filter.Eq(s => s.Country, country);
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<Supplier?> GetByEmailAsync(string email)
        {
            var filter = Builders<Supplier>.Filter.Eq(s => s.Email, email);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}