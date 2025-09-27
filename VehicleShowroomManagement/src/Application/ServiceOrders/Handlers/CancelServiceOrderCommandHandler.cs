using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

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
            if (serviceOrder == null)
                return false;

            if (serviceOrder.Status == "Completed" || serviceOrder.Status == "Cancelled")
                return false;

            serviceOrder.Status = "Cancelled";
            if (!string.IsNullOrEmpty(request.CancellationReason))
            {
                serviceOrder.Description = request.CancellationReason;
            }
            serviceOrder.UpdatedAt = DateTime.UtcNow;

            await _serviceOrderRepository.UpdateAsync(serviceOrder);
            _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
