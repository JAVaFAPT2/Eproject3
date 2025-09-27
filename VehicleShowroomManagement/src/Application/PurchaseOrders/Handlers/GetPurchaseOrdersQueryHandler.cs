using MediatR;
using VehicleShowroomManagement.Application.PurchaseOrders.DTOs;
using VehicleShowroomManagement.Application.PurchaseOrders.Queries;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Handlers
{
    /// <summary>
    /// Handler for getting purchase orders
    /// </summary>
    public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, IEnumerable<PurchaseOrderDto>>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

        public GetPurchaseOrdersQueryHandler(IRepository<PurchaseOrder> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
        {
            var purchaseOrders = await _purchaseOrderRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                purchaseOrders = purchaseOrders.Where(po => 
                    po.OrderNumber.Contains(request.SearchTerm!, StringComparison.OrdinalIgnoreCase) ||
                    po.Name.Contains(request.SearchTerm!, StringComparison.OrdinalIgnoreCase) ||
                    po.Brand.Contains(request.SearchTerm!, StringComparison.OrdinalIgnoreCase) ||
                    po.ModelNumber.Contains(request.SearchTerm!, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                purchaseOrders = purchaseOrders.Where(po => po.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.Brand))
            {
                purchaseOrders = purchaseOrders.Where(po => po.Brand == request.Brand);
            }

            if (request.FromDate.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(po => po.OrderDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(po => po.OrderDate <= request.ToDate.Value);
            }

            // Apply pagination
            purchaseOrders = purchaseOrders
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return purchaseOrders.Select(MapToDto);
        }

        private static PurchaseOrderDto MapToDto(PurchaseOrder purchaseOrder)
        {
            return new PurchaseOrderDto
            {
                Id = purchaseOrder.Id,
                OrderNumber = purchaseOrder.OrderNumber,
                ModelNumber = purchaseOrder.ModelNumber,
                Name = purchaseOrder.Name,
                Brand = purchaseOrder.Brand,
                Price = purchaseOrder.Price,
                Quantity = purchaseOrder.Quantity,
                TotalAmount = purchaseOrder.TotalAmount,
                Status = purchaseOrder.Status,
                OrderDate = purchaseOrder.OrderDate,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
                ActualDeliveryDate = purchaseOrder.ActualDeliveryDate,
                SupplierId = purchaseOrder.SupplierId,
                SupplierName = purchaseOrder.SupplierName,
                Notes = purchaseOrder.Notes,
                CreatedBy = purchaseOrder.CreatedBy,
                CreatedAt = purchaseOrder.CreatedAt,
                UpdatedAt = purchaseOrder.UpdatedAt
            };
        }
    }
}
