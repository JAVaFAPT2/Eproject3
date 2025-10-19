namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById
{
    /// <summary>
    /// Query for getting a single purchase order by ID with lines included
    /// </summary>
    public record GetPurchaseOrderByIdQuery(string Id) : IRequest<PurchaseOrderDetailDto?>;

    /// <summary>
    /// Purchase order detail DTO with lines included
    /// </summary>
    public class PurchaseOrderDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public List<PurchaseOrderLineDto> Lines { get; set; } = new List<PurchaseOrderLineDto>();
    }

    /// <summary>
    /// Purchase order line DTO
    /// </summary>
    public class PurchaseOrderLineDto
    {
        public string Id { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal LineTotal { get; set; }
    }
}
