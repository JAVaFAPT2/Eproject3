using System;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// Domain event raised when a vehicle is sold
    /// </summary>
    public class VehicleSoldEvent : DomainEvent
    {
        public int VehicleId { get; }
        public int SalesOrderId { get; }
        public int CustomerId { get; }
        public decimal SalePrice { get; }

        public VehicleSoldEvent(int vehicleId, int salesOrderId, int customerId, decimal salePrice)
        {
            VehicleId = vehicleId;
            SalesOrderId = salesOrderId;
            CustomerId = customerId;
            SalePrice = salePrice;
        }
    }
}