using System;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;


namespace VehicleShowroomManagement.Application.Orders.Commands
{
    /// <summary>
    /// Command for creating a new order
    /// </summary>
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public CustomerInfo Customer { get; set; }
        public string VehicleId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public CreateOrderCommand(
            CustomerInfo customer,
            string vehicleId,
            decimal totalAmount,
            string paymentMethod)
        {
            Customer = customer;
            VehicleId = vehicleId;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
        }
    }

    /// <summary>
    /// Command for printing an order
    /// </summary>
    public class PrintOrderCommand : IRequest<OrderPrintDto>
    {
        public string OrderId { get; set; }

        public PrintOrderCommand(string orderId)
        {
            OrderId = orderId;
        }
    }
}
