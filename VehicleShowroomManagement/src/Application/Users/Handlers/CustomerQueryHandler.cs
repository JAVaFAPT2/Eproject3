using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Users.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Users.Handlers
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
            var allCustomers = await _customerRepository.GetAllAsync();
            var customers = allCustomers.Where(c => !c.IsDeleted).ToList();

            // Apply search filter
            var filteredCustomers = customers.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredCustomers = filteredCustomers.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    c.Email.ToLower().Contains(searchTerm) ||
                    c.Phone.ToLower().Contains(searchTerm));
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
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                City = "N/A", // Not available in new schema
                State = "N/A", // Not available in new schema
                ZipCode = "N/A", // Not available in new schema
                Cccd = "N/A", // Not available in new schema
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
            var allOrders = await _orderRepository.GetAllAsync();
            var orders = allOrders.Where(o => o.CustomerId == request.CustomerId && !o.IsDeleted).ToList();

            return orders.Select(MapToDto);
        }

        private static OrderDto MapToDto(SalesOrder order)
        {
            return new OrderDto
            {
                Id = order.Id.ToString(),
                OrderNumber = order.SalesOrderId,
                CustomerId = order.CustomerId,
                Customer = new CustomerInfo
                {
                    CustomerId = order.CustomerId,
                    Name = "Customer Name",
                    Email = "customer@example.com",
                    Phone = "0901234567",
                    Address = "Customer Address"
                },
                VehicleId = order.VehicleId,
                Vehicle = new VehicleInfo
                {
                    VehicleId = order.VehicleId,
                    VIN = order.VehicleId,
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
}
