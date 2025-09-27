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
            if (serviceOrder == null)
                return false;

            if (serviceOrder.Status != "InProgress")
                return false;

            serviceOrder.CompleteService();
            if (!string.IsNullOrEmpty(request.CompletionNotes))
            {
                serviceOrder.Description = request.CompletionNotes;
            }

            await _serviceOrderRepository.UpdateAsync(serviceOrder);
            _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
