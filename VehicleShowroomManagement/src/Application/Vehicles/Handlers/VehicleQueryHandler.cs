using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Vehicles.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Vehicles.Handlers
{
    /// <summary>
    /// Handler for getting vehicles with filtering and pagination
    /// </summary>
    public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, IEnumerable<VehicleDto>>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public GetVehiclesQueryHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var vehicles = allVehicles.Where(v => !v.IsDeleted).ToList();

            // Apply filters
            var filteredVehicles = vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredVehicles = filteredVehicles.Where(v =>
                    v.VehicleId.ToLower().Contains(searchTerm) ||
                    v.ModelNumber.ToLower().Contains(searchTerm) ||
                    v.ExternalNumber.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                filteredVehicles = filteredVehicles.Where(v => v.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.Brand))
            {
                // Brand filtering not directly available in new schema
                // Could be implemented by joining with VehicleModel
                filteredVehicles = filteredVehicles.Where(v => v.ModelNumber.Contains(request.Brand));
            }

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedVehicles = filteredVehicles.Skip(skip).Take(request.PageSize);

            return paginatedVehicles.Select(MapToDto).ToList();
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                VehicleId = vehicle.VehicleId,
                VIN = vehicle.RegistrationData?.VIN ?? vehicle.VehicleId,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                Name = vehicle.ModelNumber, // Using ModelNumber as name
                Brand = "Unknown", // Not available in new schema
                BrandId = string.Empty,
                ModelId = vehicle.ModelNumber,
                Color = "Unknown", // Not available in new schema
                Year = 0, // Not available in new schema
                PurchasePrice = vehicle.PurchasePrice,
                Price = vehicle.PurchasePrice, // Backward compatibility
                Mileage = 0, // Not available in new schema
                Status = vehicle.Status,
                LicensePlate = vehicle.RegistrationData?.LicensePlate ?? "N/A",
                RegistrationNumber = vehicle.RegistrationData?.LicensePlate ?? "N/A",
                RegistrationDate = vehicle.RegistrationData?.RegistrationDate,
                ExpiryDate = vehicle.RegistrationData?.ExpiryDate,
                ExternalId = vehicle.ExternalNumber,
                Photos = vehicle.Photos ?? new List<string>(),
                ReceiptDate = vehicle.ReceiptDate,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                Images = new List<VehicleImageDto>() // Not available in new schema
            };
        }
    }

    /// <summary>
    /// Handler for getting a vehicle by ID
    /// </summary>
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public GetVehicleByIdQueryHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null || vehicle.IsDeleted)
            {
                return null;
            }

            return MapToDto(vehicle);
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                VehicleId = vehicle.VehicleId,
                VIN = vehicle.RegistrationData?.VIN ?? vehicle.VehicleId,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                Name = vehicle.ModelNumber, // Using ModelNumber as name
                Brand = "Unknown", // Not available in new schema
                BrandId = string.Empty,
                ModelId = vehicle.ModelNumber,
                Color = "Unknown", // Not available in new schema
                Year = 0, // Not available in new schema
                PurchasePrice = vehicle.PurchasePrice,
                Price = vehicle.PurchasePrice, // Backward compatibility
                Mileage = 0, // Not available in new schema
                Status = vehicle.Status,
                LicensePlate = vehicle.RegistrationData?.LicensePlate ?? "N/A",
                RegistrationNumber = vehicle.RegistrationData?.LicensePlate ?? "N/A",
                RegistrationDate = vehicle.RegistrationData?.RegistrationDate,
                ExpiryDate = vehicle.RegistrationData?.ExpiryDate,
                ExternalId = vehicle.ExternalNumber,
                Photos = vehicle.Photos ?? new List<string>(),
                ReceiptDate = vehicle.ReceiptDate,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                Images = new List<VehicleImageDto>() // Not available in new schema
            };
        }
    }
}
