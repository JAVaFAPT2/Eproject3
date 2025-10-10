using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders
{
    public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, GetPurchaseOrdersResult>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

        public GetPurchaseOrdersQueryHandler(IRepository<PurchaseOrder> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<GetPurchaseOrdersResult> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
        {
            var allPOs = await _purchaseOrderRepository.GetAllAsync();

            // Apply filters
            var filtered = allPOs.Where(po =>
                (request.Status == null || po.Status == request.Status) &&
                (request.FromDate == null || po.OrderDate >= request.FromDate) &&
                (request.ToDate == null || po.OrderDate <= request.ToDate));

            var totalCount = filtered.Count();
            
            // Apply pagination
            var pagedPOs = filtered
                .OrderByDescending(po => po.OrderDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(po => new PurchaseOrderSummaryDto
                {
                    Id = po.Id,
                    CreatedBy = po.CreatedBy,
                    OrderDate = po.OrderDate,
                    ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                    TotalAmount = po.TotalAmount,
                    Status = po.Status.ToString()
                })
                .ToList();

            return new GetPurchaseOrdersResult
            {
                PurchaseOrders = pagedPOs,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }
    }
}

