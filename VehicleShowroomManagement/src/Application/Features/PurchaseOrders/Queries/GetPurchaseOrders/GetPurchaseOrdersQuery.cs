using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders
{
    /// <summary>
    /// Query for getting purchase orders with pagination and filters
    /// </summary>
    public record GetPurchaseOrdersQuery(
        int PageNumber,
        int PageSize,
        PurchaseOrderStatus? Status,
        string? SupplierId,
        DateTime? FromDate,
        DateTime? ToDate) : IRequest<GetPurchaseOrdersResult>;

    /// <summary>
    /// Result for get purchase orders query
    /// </summary>
    public class GetPurchaseOrdersResult
    {
        public List<PurchaseOrderSummaryDto> PurchaseOrders { get; set; } = new List<PurchaseOrderSummaryDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// Purchase order summary DTO
    /// </summary>
    public class PurchaseOrderSummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int TotalItems { get; set; }
    }
}