using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.DTOs;
using VehicleShowroomManagement.Application.ServiceOrders.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.ServiceOrders.Handlers
{
    /// <summary>
    /// Handler for getting service order by ID
    /// </summary>
    public class GetServiceOrderByIdQueryHandler : IRequestHandler<GetServiceOrderByIdQuery, ServiceOrderDto?>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;

        public GetServiceOrderByIdQueryHandler(IRepository<ServiceOrder> serviceOrderRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
        }

        public async Task<ServiceOrderDto?> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(request.Id);
            if (serviceOrder == null)
                return null;

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
