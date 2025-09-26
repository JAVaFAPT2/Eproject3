using MediatR;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.ServiceOrders.Handlers
{
    /// <summary>
    /// Handler for creating service orders
    /// </summary>
    public class CreateServiceOrderCommandHandler : IRequestHandler<CreateServiceOrderCommand, string>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceOrderCommandHandler(
            IRepository<ServiceOrder> serviceOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
        {
            var serviceOrder = new ServiceOrder
            {
                VehicleId = request.VehicleId,
                CustomerId = request.CustomerId,
                ServiceDate = request.ServiceDate,
                ServiceType = request.ServiceType,
                TotalCost = request.TotalCost,
                Description = request.Description,
                Status = "Scheduled"
            };

            await _serviceOrderRepository.AddAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return serviceOrder.Id;
        }
    }
}
