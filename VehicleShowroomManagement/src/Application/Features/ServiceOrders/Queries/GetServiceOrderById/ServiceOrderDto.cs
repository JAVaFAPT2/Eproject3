using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrderById
{
    public class ServiceOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string ServiceOrderId { get; set; } = string.Empty;
        public string ServiceOrderNumber { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static ServiceOrderDto FromEntity(ServiceOrder serviceOrder)
        {
            return new ServiceOrderDto
            {
                Id = serviceOrder.Id,
                ServiceOrderId = serviceOrder.ServiceOrderId,
                ServiceOrderNumber = serviceOrder.ServiceOrderNumber,
                SalesOrderId = serviceOrder.SalesOrderId,
                EmployeeId = serviceOrder.EmployeeId,
                ServiceDate = serviceOrder.ServiceDate,
                Description = serviceOrder.Description,
                Cost = serviceOrder.Cost,
                CreatedAt = serviceOrder.CreatedAt,
                UpdatedAt = serviceOrder.UpdatedAt
            };
        }
    }
}