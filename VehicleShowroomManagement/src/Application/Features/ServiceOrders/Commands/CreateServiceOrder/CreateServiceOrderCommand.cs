using MediatR;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder
{
    public class CreateServiceOrderCommand : IRequest<string>
    {
        public string ServiceOrderNumber { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public string? Notes { get; set; }
    }
}