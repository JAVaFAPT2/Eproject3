using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Orders.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Orders.Handlers
{
    /// <summary>
    /// Handler for getting orders with pagination
    /// </summary>
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetOrdersQueryHandler(IRepository<SalesOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;

            var allOrders = await _orderRepository.GetAllAsync();
            var orders = allOrders.Where(o => !o.IsDeleted)
                .Skip(skip)
                .Take(request.PageSize)
                .ToList();

            return orders.Select(MapToDto);
        }

        private static OrderDto MapToDto(SalesOrder order)
        {
            var orderId = order.Id ?? "UNKNOWN";
            return new OrderDto
            {
                Id = orderId,
                OrderNumber = order.SalesOrderId,
                CustomerId = order.CustomerId,
                Customer = new CustomerInfo
                {
                    CustomerId = order.CustomerId,
                    Name = "Customer Name", // Would need to populate from customer entity
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

    /// <summary>
    /// Handler for getting an order by ID
    /// </summary>
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetOrderByIdQueryHandler(IRepository<SalesOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null || order.IsDeleted)
            {
                return null;
            }

            return MapToDto(order);
        }

        private static OrderDto MapToDto(SalesOrder order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = $"ORD-{order.OrderDate:yyyyMMdd}-{(order.Id ?? "UNKNOWN").Substring(0, Math.Min(4, (order.Id ?? "UNKNOWN").Length))}",
                CustomerId = order.CustomerId,
                Customer = new CustomerInfo
                {
                    Id = order.CustomerId,
                    FirstName = "Customer", // Would need to populate from customer entity
                    LastName = "Name",
                    Email = "customer@example.com",
                    Phone = "0901234567"
                },
                VehicleId = "VehicleId", // Would need to populate from order items
                Vehicle = new VehicleInfo
                {
                    VehicleId = "VehicleId",
                    Name = "Vehicle Name",
                    VIN = "VIN123",
                    Brand = "Brand Name",
                    Price = order.SalePrice
                },
                Items = new List<OrderItemDto>(), // Would need to populate from order items
                TotalAmount = order.SalePrice,
                OrderDate = order.OrderDate,
                PaymentMethod = "Cash", // Would need to populate from order data
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}
