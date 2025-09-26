using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Bson;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.WebAPI.Controllers;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for creating a new order
    /// </summary>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IRepository<SalesOrder> _orderRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<User> _userRepository;

        public CreateOrderCommandHandler(
            IRepository<SalesOrder> orderRepository,
            IRepository<Customer> customerRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<User> userRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Check if customer exists, create if not
            var customer = await _customerRepository.FirstOrDefaultAsync(c =>
                c.Email == request.Customer.Email && !c.IsDeleted);

            if (customer == null)
            {
                customer = new Customer
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    FirstName = request.Customer.Name.Split(' ')[0],
                    LastName = request.Customer.Name.Split(' ').Length > 1 ? request.Customer.Name.Split(' ')[1] : "",
                    Email = request.Customer.Email,
                    Phone = request.Customer.Phone,
                    Address = request.Customer.Address,
                    City = "Unknown",
                    State = "Unknown",
                    ZipCode = "Unknown",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await _customerRepository.AddAsync(customer);
            }

            // Get current user (sales person)
            var salesPerson = await _userRepository.FirstOrDefaultAsync(u => !u.IsDeleted && u.IsActive);
            if (salesPerson == null)
            {
                throw new InvalidOperationException("No active sales person found");
            }

            // Generate order number
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

            // Create sales order
            var order = new SalesOrder
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CustomerId = customer.Id,
                SalesPersonId = salesPerson.Id,
                Status = "CONFIRMED",
                OrderDate = DateTime.UtcNow,
                TotalAmount = request.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                SalesOrderItems = new List<SalesOrderItem>
                {
                    new SalesOrderItem
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        SalesOrderId = ObjectId.GenerateNewId().ToString(), // Will be set properly after order creation
                        VehicleId = request.VehicleId,
                        UnitPrice = request.TotalAmount,
                        Discount = 0,
                        LineTotal = request.TotalAmount,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                }
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return MapToDto(order);
        }

        private static OrderDto MapToDto(SalesOrder order)
        {
            return new OrderDto
            {
                Id = order.SalesOrderId.ToString(),
                OrderNumber = $"ORD-{order.OrderDate:yyyyMMdd}-{order.SalesOrderId.ToString().Substring(0, 4)}",
                CustomerId = order.CustomerId,
                Customer = new CustomerInfo
                {
                    CustomerId = order.CustomerId,
                    Name = "Customer Name", // Would need to populate from customer entity
                    Email = "customer@example.com",
                    Phone = "0901234567",
                    Address = "Customer Address"
                },
                VehicleId = "VehicleId", // Would need to populate from order items
                Vehicle = new VehicleInfo
                {
                    VehicleId = "VehicleId",
                    VIN = "VIN123",
                    ModelNumber = "MODEL001",
                    Name = "Vehicle Name",
                    Brand = "Brand Name",
                    Price = 50000
                },
                SalesPersonId = order.SalesPersonId,
                SalesPerson = new UserInfo
                {
                    UserId = order.SalesPersonId,
                    Username = "salesperson",
                    FullName = "Sales Person",
                    Email = "sales@example.com"
                },
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                PaymentMethod = "CASH",
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }

    /// <summary>
    /// Handler for printing an order
    /// </summary>
    public class PrintOrderCommandHandler : IRequestHandler<PrintOrderCommand, OrderPrintDto>
    {
        private readonly IRepository<SalesOrder> _orderRepository;

        public PrintOrderCommandHandler(IRepository<SalesOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderPrintDto> Handle(PrintOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            return new OrderPrintDto
            {
                OrderNumber = $"ORD-{order.OrderDate:yyyyMMdd}-{request.OrderId.Substring(0, 4)}",
                Customer = new CustomerInfo
                {
                    Name = "Customer Name",
                    Email = "customer@example.com",
                    Phone = "0901234567",
                    Address = "Customer Address"
                },
                Vehicle = new VehicleInfo
                {
                    Name = "Vehicle Name",
                    Brand = "Brand Name",
                    Price = order.TotalAmount
                },
                SalesPerson = new UserInfo
                {
                    FullName = "Sales Person",
                    Email = "sales@example.com"
                },
                TotalAmount = order.TotalAmount,
                PaymentMethod = "CASH",
                OrderDate = order.OrderDate,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        VehicleId = "VehicleId",
                        Quantity = 1,
                        UnitPrice = order.TotalAmount,
                        LineTotal = order.TotalAmount
                    }
                },
                CompanyInfo = "Vehicle Showroom Management System"
            };
        }
    }
}