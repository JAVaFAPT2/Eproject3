namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<VehicleModel> modelRepository) : IRequestHandler<CreateOrderCommand, string>
    {
        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate input parameters first
            if (string.IsNullOrWhiteSpace(request.CustomerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(request.CustomerId));
            
            if (string.IsNullOrWhiteSpace(request.ModelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(request.ModelNumber));
            
            if (request.SalePrice < 0)
                throw new ArgumentException("Sale price cannot be negative", nameof(request.SalePrice));

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
