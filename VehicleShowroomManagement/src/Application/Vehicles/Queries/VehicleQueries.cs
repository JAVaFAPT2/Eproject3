using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Vehicles.Queries
{
    /// <summary>
    /// Query for getting vehicles with filtering and pagination
    /// </summary>
    public class GetVehiclesQuery : IRequest<IEnumerable<VehicleDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Brand { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetVehiclesQuery(string? searchTerm = null, string? status = null, string? brand = null, int pageNumber = 1, int pageSize = 10)
        {
            SearchTerm = searchTerm;
            Status = status;
            Brand = brand;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Query for getting a vehicle by ID
    /// </summary>
    public class GetVehicleByIdQuery : IRequest<VehicleDto?>
    {
        public string VehicleId { get; set; }

        public GetVehicleByIdQuery(string vehicleId)
        {
            VehicleId = vehicleId;
        }
    }
}
