using MediatR;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders
{
    /// <summary>
    /// Query for getting recent orders
    /// </summary>
    public record GetRecentOrdersQuery(int Limit) : IRequest<List<RecentOrderDto>>;

    /// <summary>
    /// Recent order DTO
    /// </summary>
    public class RecentOrderDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string SalesPersonName { get; set; } = string.Empty;
    }
}