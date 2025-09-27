using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrderById
{
    public class GetServiceOrderByIdQueryHandler : IRequestHandler<GetServiceOrderByIdQuery, ServiceOrderDto>
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;

        public GetServiceOrderByIdQueryHandler(IServiceOrderRepository serviceOrderRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
        }

        public async Task<ServiceOrderDto> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);

            if (serviceOrder == null)
                throw new KeyNotFoundException($"Service order with ID {request.ServiceOrderId} not found");

            return ServiceOrderDto.FromEntity(serviceOrder);
        }
    }
}