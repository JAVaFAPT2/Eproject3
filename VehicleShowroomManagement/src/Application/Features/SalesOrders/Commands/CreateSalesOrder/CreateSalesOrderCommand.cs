using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.CreateSalesOrder
{
    /// <summary>
    /// Command to create a sales order - core business process
    /// </summary>
    public record CreateSalesOrderCommand : IRequest<string>
    {
        public string OrderNumber { get; init; }
        public string CustomerId { get; init; }
        public string VehicleId { get; init; }
        public string SalesPersonId { get; init; }
        public decimal TotalAmount { get; init; }
        public PaymentMethod PaymentMethod { get; init; }

        public CreateSalesOrderCommand(
            string orderNumber,
            string customerId,
            string vehicleId,
            string salesPersonId,
            decimal totalAmount,
            PaymentMethod paymentMethod)
        {
            OrderNumber = orderNumber;
            CustomerId = customerId;
            VehicleId = vehicleId;
            SalesPersonId = salesPersonId;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
        }
    }
}