using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.ServiceOrders.Handlers
{
    /// <summary>
    /// Handler for completing service orders
    /// </summary>
    public class CompleteServiceOrderCommandHandler : IRequestHandler<CompleteServiceOrderCommand, bool>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteServiceOrderCommandHandler(
            IRepository<ServiceOrder> serviceOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CompleteServiceOrderCommand request, CancellationToken cancellationToken)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(request.Id);
            if (serviceOrder == null || serviceOrder.IsDeleted)
                return false;

            // Update completion notes if provided
            if (!string.IsNullOrEmpty(request.CompletionNotes))
            {
                serviceOrder.UpdateDescription(request.CompletionNotes);
            }

            await _serviceOrderRepository.UpdateAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
