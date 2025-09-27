using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByInvoiceIdAsync(string invoiceId);
        Task<IEnumerable<Payment>> GetBySalesOrderIdAsync(string salesOrderId);
        Task<IEnumerable<Payment>> GetByPaymentMethodAsync(string paymentMethod);
        Task<IEnumerable<Payment>> GetByStatusAsync(string status);
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalPaymentsByInvoiceAsync(string invoiceId);
    }
}