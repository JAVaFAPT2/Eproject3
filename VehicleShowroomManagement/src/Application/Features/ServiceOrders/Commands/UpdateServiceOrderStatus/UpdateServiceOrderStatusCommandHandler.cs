using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateServiceOrderStatus
{
    public class UpdateServiceOrderStatusCommandHandler(
        IRepository<ServiceOrder> serviceOrderRepository) : IRequestHandler<UpdateServiceOrderStatusCommand>
    {
        public async Task Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var serviceOrder = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId, cancellationToken)
                ?? throw new KeyNotFoundException("Service order not found");

            serviceOrder.UpdateStatus(request.Status, request.LicensePlate);
            await serviceOrderRepository.UpdateAsync(serviceOrder, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.LicensePlate))
            {
                // Attempt to set license plate on assigned vehicle (if any) via order linkage is out of scope here
                // This implementation only persists license plate on the service order as requested.
            }
        }
    }
}


