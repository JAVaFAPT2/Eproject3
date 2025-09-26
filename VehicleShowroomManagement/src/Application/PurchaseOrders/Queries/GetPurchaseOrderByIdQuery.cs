using MediatR;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Queries
{
    /// <summary>
    /// Query to get a purchase order by ID
    /// </summary>
    public class GetPurchaseOrderByIdQuery : IRequest<PurchaseOrderDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}
