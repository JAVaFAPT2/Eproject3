using MediatR;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Queries
{
    /// <summary>
    /// Query to get all purchase orders with filtering and pagination
    /// </summary>
    public class GetPurchaseOrdersQuery : IRequest<IEnumerable<PurchaseOrderDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Brand { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
