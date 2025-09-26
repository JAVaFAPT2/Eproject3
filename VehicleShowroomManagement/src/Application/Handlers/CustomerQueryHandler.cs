using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for getting customers with search and pagination
    /// </summary>
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly IRepository<Customer> _customerRepository;

        public GetCustomersQueryHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllQueryable()
                .Where(c => !c.IsDeleted)
                .ToListAsync(cancellationToken);

            // Apply search filter
            var filteredCustomers = customers.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredCustomers = filteredCustomers.Where(c =>
                    c.FirstName.ToLower().Contains(searchTerm) ||
                    c.LastName.ToLower().Contains(searchTerm) ||
                    c.Email.ToLower().Contains(searchTerm) ||
                    (c.Phone ?? "").ToLower().Contains(searchTerm));
            }

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedCustomers = filteredCustomers.Skip(skip).Take(request.PageSize);

            return paginatedCustomers.Select(MapToDto).ToList();
        }

        private static CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                Cccd = "CCCD123456789", // Placeholder - would need proper field in Customer entity
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt,
                Orders = new List<OrderDto>() // Would need to populate from related orders
            };
        }
    }

    /// <summary>
    /// Handler for getting orders by customer
    /// </summary>
    public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetCustomerOrdersQueryHandler(IRepository<SalesOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllQueryable()
                .Where(o => o.CustomerId == request.CustomerId && !o.IsDeleted)
                .ToListAsync(cancellationToken);

            return orders.Select(MapToDto).ToList();
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
                    Name = "Customer Name",
                    Email = "customer@example.com",
                    Phone = "0901234567",
                    Address = "Customer Address"
                },
                VehicleId = "VehicleId",
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
}