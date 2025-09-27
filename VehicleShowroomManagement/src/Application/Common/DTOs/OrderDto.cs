using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Sales Orders
    /// </summary>
    public class OrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public string VehicleId { get; set; } = string.Empty;
        public string VehicleDetails { get; set; } = string.Empty;
        public VehicleInfo Vehicle { get; set; } = new VehicleInfo();
        public string SalesPersonId { get; set; } = string.Empty;
        public string SalesPersonName { get; set; } = string.Empty;
        public UserInfo SalesPerson { get; set; } = new UserInfo();
        public string Status { get; set; } = "DRAFT";
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }


    /// <summary>
    /// Vehicle information for orders
    /// </summary>
    public class VehicleInfo
    {
        public string VehicleId { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    /// <summary>
    /// User information for orders
    /// </summary>
    public class UserInfo
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Order item information
    /// </summary>
    public class OrderItemDto
    {
        public string ItemId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }
    }

    /// <summary>
    /// DTO for order printing
    /// </summary>
    public class OrderPrintDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public VehicleInfo Vehicle { get; set; } = new VehicleInfo();
        public UserInfo SalesPerson { get; set; } = new UserInfo();
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public string CompanyInfo { get; set; } = "Vehicle Showroom Management System";
    }
}
