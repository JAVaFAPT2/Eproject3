using MediatR;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicles
{
    /// <summary>
    /// Query for getting vehicles with pagination and filters
    /// </summary>
    public record GetVehiclesQuery(
        int PageNumber,
        int PageSize,
        VehicleStatus? Status,
        string? Brand) : IRequest<GetVehiclesResult>;

    /// <summary>
    /// Result for get vehicles query
    /// </summary>
    public class GetVehiclesResult
    {
        public List<VehicleDto> Vehicles { get; set; } = new List<VehicleDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}