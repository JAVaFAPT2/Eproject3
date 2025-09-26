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
            var orders = await _orderRepository.GetAllQueryable()
                .Where(o => !o.IsDeleted)
                .ToListAsync(cancellationToken);

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedOrders = orders.Skip(skip).Take(request.PageSize);

            return paginatedOrders.Select(MapToDto).ToList();
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
}