using MediatR;
using MongoDB.Bson;
using VehicleShowroomManagement.Application.Orders.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Email.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Orders.Handlers
{
    /// <summary>
    /// Handler for creating a new order
    /// </summary>
    public class CreateOrderCommandHandler(
        IRepository<SalesOrder> orderRepository,
        IRepository<Customer> customerRepository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<Employee> employeeRepository,
        IMediator mediator)
        : IRequestHandler<CreateOrderCommand, OrderDto>
    {

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Check if customer exists, create if not
            var customer = await customerRepository.FirstOrDefaultAsync(c =>
                c.Email == request.Customer.Email && !c.IsDeleted);

            if (customer == null)
            {
                customer = new Customer
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CustomerId = $"CUST-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}",
                    Name = request.Customer.Name,
                    Email = request.Customer.Email,
                    Phone = request.Customer.Phone,
                    Address = request.Customer.Address,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await customerRepository.AddAsync(customer);
            }

            // Get current employee (sales person)
            var salesPerson = await employeeRepository.FirstOrDefaultAsync(e => !e.IsDeleted && e.IsActive);
            if (salesPerson == null)
            {
                throw new InvalidOperationException("No active sales person found");
            }

            // Get vehicle information
            var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                throw new InvalidOperationException("Vehicle not found");
            }

            // Create sales order
            var order = new SalesOrder
            {
                Id = ObjectId.GenerateNewId().ToString(),
                SalesOrderId = $"SO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                CustomerId = customer.Id,
                VehicleId = request.VehicleId,
                EmployeeId = salesPerson.Id,
                OrderDate = DateTime.UtcNow,
                SalePrice = request.TotalAmount,
                Status = "Confirmed",
                DataSheetOutput = null,
                ConfirmationOutput = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await orderRepository.AddAsync(order);
            await orderRepository.SaveChangesAsync();

            // Send order confirmation email
            try
            {
                var emailCommand = new SendOrderConfirmationEmailCommand
                {
                    CustomerEmail = customer.Email,
                    CustomerName = customer.Name,
                    OrderNumber = order.SalesOrderId,
                    VehicleName = vehicle.ModelNumber,
                    VehicleBrand = "Brand", // Would need to get from VehicleModel
                    TotalAmount = request.TotalAmount,
                    OrderDate = order.OrderDate,
                    Status = order.Status
                };

                await mediator.Send(emailCommand, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log email error but don't fail the order creation
                // In a real application, you might want to use a logging service here
                Console.WriteLine($"Failed to send order confirmation email: {ex.Message}");
            }

            return MapToDto(order);
        }

        private static OrderDto MapToDto(SalesOrder order)
        {
            return new OrderDto
            {
                Id = order.Id ?? "UNKNOWN",
                OrderNumber = order.SalesOrderId,
                CustomerId = order.CustomerId,
                Customer = new CustomerInfo
                {
                    Id = order.CustomerId,
                    FirstName = "Customer", // Would need to populate from customer entity
                    LastName = "Name",
                    Email = "customer@example.com",
                    Phone = "0901234567"
                },
                VehicleId = order.VehicleId,
                Vehicle = new VehicleInfo
                {
                    VehicleId = order.VehicleId,
                    VIN = "VIN123",
                    ModelNumber = "MODEL001",
                    Name = "Vehicle Name",
                    Brand = "Brand Name",
                    Price = order.SalePrice
                },
                SalesPersonId = order.EmployeeId,
                SalesPerson = new UserInfo
                {
                    UserId = order.EmployeeId,
                    Username = "employee",
                    FullName = "Employee",
                    Email = "employee@example.com"
                },
                Status = order.Status,
                TotalAmount = order.SalePrice,
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
                    Id = "customer-id",
                    FirstName = "Customer",
                    LastName = "Name",
                    Email = "customer@example.com",
                    Phone = "0901234567"
                },
                Vehicle = new VehicleInfo
                {
                    Name = "Vehicle Name",
                    Brand = "Brand Name",
                    Price = order.SalePrice
                },
                SalesPerson = new UserInfo
                {
                    FullName = "Employee",
                    Email = "employee@example.com"
                },
                TotalAmount = order.SalePrice,
                PaymentMethod = "CASH",
                OrderDate = order.OrderDate,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        VehicleId = "VehicleId",
                        Quantity = 1,
                        UnitPrice = order.SalePrice,
                        LineTotal = order.SalePrice
                    }
                },
                CompanyInfo = "Vehicle Showroom Management System"
            };
        }
    }
}
