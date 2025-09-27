using MediatR;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById
{
    public class GetPurchaseOrderByIdQuery : IRequest<PurchaseOrderDto>
    {
        public string PurchaseOrderId { get; set; } = string.Empty;
    }
}