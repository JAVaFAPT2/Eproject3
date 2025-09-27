using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Queries.GetOrders
{
    /// <summary>
    /// Query for getting sales orders with pagination and filters
    /// </summary>
    public record GetOrdersQuery(
        int PageNumber,
        int PageSize,
        OrderStatus? Status,
        string? CustomerId,
        DateTime? FromDate,
        DateTime? ToDate) : IRequest<GetOrdersResult>;

    /// <summary>
    /// Result for get orders query
    /// </summary>
    public class GetOrdersResult
    {
        public List<OrderSummaryDto> Orders { get; set; } = new List<OrderSummaryDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// Order summary DTO
    /// </summary>
    public class OrderSummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string SalesPersonId { get; set; } = string.Empty;
        public string SalesPersonName { get; set; } = string.Empty;
    }
}