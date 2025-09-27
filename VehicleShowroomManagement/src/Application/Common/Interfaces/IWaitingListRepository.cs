using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IWaitingListRepository : IRepository<WaitingList>
    {
        Task<IEnumerable<WaitingList>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<WaitingList>> GetByModelNumberAsync(string modelNumber);
        Task<IEnumerable<WaitingList>> GetByStatusAsync(string status);
        Task<WaitingList?> GetByCustomerAndModelAsync(string customerId, string modelNumber);
    }
}