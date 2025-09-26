using System;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// Domain event raised when a vehicle is sold
    /// </summary>
    public class VehicleSoldEvent : DomainEvent
    {
        public string VehicleId { get; }
        public string SalesOrderId { get; }
        public string CustomerId { get; }
        public decimal SalePrice { get; }

        public VehicleSoldEvent(string vehicleId, string salesOrderId, string customerId, decimal salePrice)
        {
            VehicleId = vehicleId;
            SalesOrderId = salesOrderId;
            CustomerId = customerId;
            SalePrice = salePrice;
        }
    }
}