using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.DTOs;
using VehicleShowroomManagement.Application.ServiceOrders.Queries;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.ServiceOrders.Handlers
{
    /// <summary>
    /// Handler for getting service orders
    /// </summary>
    public class GetServiceOrdersQueryHandler : IRequestHandler<GetServiceOrdersQuery, IEnumerable<ServiceOrderDto>>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;

        public GetServiceOrdersQueryHandler(IRepository<ServiceOrder> serviceOrderRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
        }

        public async Task<IEnumerable<ServiceOrderDto>> Handle(GetServiceOrdersQuery request, CancellationToken cancellationToken)
        {
            var serviceOrders = await _serviceOrderRepository.GetAllAsync();

            // Apply filters (simplified for new schema)
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                serviceOrders = serviceOrders.Where(so =>
                    so.Description != null && so.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    so.ServiceOrderId.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    so.SalesOrderId.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Status and ServiceType filtering not available in new schema

            if (request.FromDate.HasValue)
            {
                serviceOrders = serviceOrders.Where(so => so.ServiceDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                serviceOrders = serviceOrders.Where(so => so.ServiceDate <= request.ToDate.Value);
            }

            // Apply pagination
            serviceOrders = serviceOrders
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return serviceOrders.Select(MapToDto);
        }

        private static ServiceOrderDto MapToDto(ServiceOrder serviceOrder)
        {
            return new ServiceOrderDto
            {
                Id = serviceOrder.Id,
                ServiceOrderId = serviceOrder.ServiceOrderId,
                SalesOrderId = serviceOrder.SalesOrderId,
                EmployeeId = serviceOrder.EmployeeId,
                ServiceDate = serviceOrder.ServiceDate,
                Description = serviceOrder.Description,
                Cost = serviceOrder.Cost,
                CreatedAt = serviceOrder.CreatedAt,
                UpdatedAt = serviceOrder.UpdatedAt
            };
        }
    }
}
