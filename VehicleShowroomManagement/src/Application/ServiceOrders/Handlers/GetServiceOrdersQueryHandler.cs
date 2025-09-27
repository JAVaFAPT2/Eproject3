using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.DTOs;
using VehicleShowroomManagement.Application.ServiceOrders.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

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

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                serviceOrders = serviceOrders.Where(so => 
                    so.Description != null && so.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    so.ServiceType.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    so.VehicleId.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    so.CustomerId.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                serviceOrders = serviceOrders.Where(so => so.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.ServiceType))
            {
                serviceOrders = serviceOrders.Where(so => so.ServiceType == request.ServiceType);
            }

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
                VehicleId = serviceOrder.VehicleId,
                CustomerId = serviceOrder.CustomerId,
                ServiceDate = serviceOrder.ServiceDate,
                Status = serviceOrder.Status,
                TotalCost = serviceOrder.TotalCost,
                Description = serviceOrder.Description,
                ServiceType = serviceOrder.ServiceType,
                CreatedAt = serviceOrder.CreatedAt,
                UpdatedAt = serviceOrder.UpdatedAt
            };
        }
    }
}
