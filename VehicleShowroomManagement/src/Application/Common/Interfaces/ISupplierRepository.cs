using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier?> GetByCompanyNameAsync(string companyName);
        Task<IEnumerable<Supplier>> GetByCountryAsync(string country);
        Task<Supplier?> GetByEmailAsync(string email);
    }
}