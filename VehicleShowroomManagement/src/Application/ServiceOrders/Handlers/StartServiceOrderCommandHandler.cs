using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.ServiceOrders.Handlers
{
    /// <summary>
    /// Handler for starting service orders
    /// </summary>
    public class StartServiceOrderCommandHandler : IRequestHandler<StartServiceOrderCommand, bool>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartServiceOrderCommandHandler(
            IRepository<ServiceOrder> serviceOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(StartServiceOrderCommand request, CancellationToken cancellationToken)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(request.Id);
            if (serviceOrder == null || serviceOrder.IsDeleted)
                return false;

            // Update service date to current time to indicate service has started
            serviceOrder.UpdateServiceDate(DateTime.UtcNow);

            await _serviceOrderRepository.UpdateAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
