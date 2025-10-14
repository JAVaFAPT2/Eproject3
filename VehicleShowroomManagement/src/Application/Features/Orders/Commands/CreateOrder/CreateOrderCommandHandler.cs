namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<VehicleModel> modelRepository) : IRequestHandler<CreateOrderCommand, string>
    {
        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Verify model exists
            var models = await modelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber, cancellationToken);
            if (!models.Any())
            {
                throw new InvalidOperationException("Vehicle model not found");
            }

            // Create order without assigning dealer or vehicle; these are set later in workflow
            var order = new Order(
                request.CustomerId,
                request.ModelNumber,
                request.SalePrice);

            await orderRepository.AddAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
