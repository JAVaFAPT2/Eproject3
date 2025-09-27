using MediatR;
using VehicleShowroomManagement.Application.ServiceOrders.DTOs;

namespace VehicleShowroomManagement.Application.ServiceOrders.Queries
{
    /// <summary>
    /// Query to get all service orders with filtering and pagination
    /// </summary>
    public class GetServiceOrdersQuery : IRequest<IEnumerable<ServiceOrderDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? ServiceType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
