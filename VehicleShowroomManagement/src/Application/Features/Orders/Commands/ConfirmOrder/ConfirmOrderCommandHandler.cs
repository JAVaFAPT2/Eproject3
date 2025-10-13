namespace VehicleShowroomManagement.Application.Features.Orders.Commands.ConfirmOrder
{
    public class ConfirmOrderCommandHandler(IRepository<Order> orderRepository) : IRequestHandler<ConfirmOrderCommand, bool>
    {
        public async Task<bool> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            order.Confirm();
            await orderRepository.UpdateAsync(order, cancellationToken);

            return true;
        }
    }
}
