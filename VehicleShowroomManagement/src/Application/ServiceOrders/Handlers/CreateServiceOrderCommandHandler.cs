using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;

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
                ServiceOrderId = $"SO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                SalesOrderId = request.SalesOrderId,
                EmployeeId = request.EmployeeId,
                ServiceDate = request.ServiceDate,
                Description = request.Description,
                Cost = request.Cost
            };

            await _serviceOrderRepository.AddAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return serviceOrder.Id;
        }
    }
}
