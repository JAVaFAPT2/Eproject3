using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.ServiceOrders.Handlers
{
    /// <summary>
    /// Handler for cancelling service orders
    /// </summary>
    public class CancelServiceOrderCommandHandler : IRequestHandler<CancelServiceOrderCommand, bool>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelServiceOrderCommandHandler(
            IRepository<ServiceOrder> serviceOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CancelServiceOrderCommand request, CancellationToken cancellationToken)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(request.Id);
            if (serviceOrder == null || serviceOrder.IsDeleted)
                return false;

            // Soft delete the service order to cancel it
            serviceOrder.SoftDelete();

            await _serviceOrderRepository.UpdateAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
