using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Handlers
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
                    v.VIN.ToLower().Contains(searchTerm) ||
                    (v.Model?.ModelName ?? "").ToLower().Contains(searchTerm) ||
                    (v.Model?.Brand?.BrandName ?? "").ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                filteredVehicles = filteredVehicles.Where(v => v.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.Brand))
            {
                filteredVehicles = filteredVehicles.Where(v =>
                    v.Model?.Brand?.BrandName.ToLower() == request.Brand.ToLower());
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
                VIN = vehicle.VIN,
                ModelNumber = vehicle.VIN,
                Name = vehicle.Model?.ModelName ?? "Unknown",
                Brand = vehicle.Model?.Brand?.BrandName ?? "Unknown",
                BrandId = vehicle.Model?.BrandId ?? string.Empty,
                ModelId = vehicle.ModelId,
                Color = vehicle.Color,
                Year = vehicle.Year,
                Price = vehicle.Price,
                Mileage = vehicle.Mileage,
                Status = vehicle.Status,
                RegistrationNumber = "TEMP-001", // Placeholder
                RegistrationDate = DateTime.UtcNow,
                ExternalId = "EXT-001", // Placeholder
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                Images = vehicle.VehicleImages.Select(img => new VehicleImageDto
                {
                    ImageId = img.ImageId.ToString(),
                    ImageUrl = img.ImageUrl,
                    ImageType = img.ImageType,
                    FileName = img.FileName,
                    FileSize = img.FileSize,
                    UploadedAt = img.UploadedAt
                }).ToList()
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
                VIN = vehicle.VIN,
                ModelNumber = vehicle.VIN,
                Name = vehicle.Model?.ModelName ?? "Unknown",
                Brand = vehicle.Model?.Brand?.BrandName ?? "Unknown",
                BrandId = vehicle.Model?.BrandId ?? string.Empty,
                ModelId = vehicle.ModelId,
                Color = vehicle.Color,
                Year = vehicle.Year,
                Price = vehicle.Price,
                Mileage = vehicle.Mileage,
                Status = vehicle.Status,
                RegistrationNumber = "TEMP-001", // Placeholder
                RegistrationDate = DateTime.UtcNow,
                ExternalId = "EXT-001", // Placeholder
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                Images = vehicle.VehicleImages.Select(img => new VehicleImageDto
                {
                    ImageId = img.ImageId.ToString(),
                    ImageUrl = img.ImageUrl,
                    ImageType = img.ImageType,
                    FileName = img.FileName,
                    FileSize = img.FileSize,
                    UploadedAt = img.UploadedAt
                }).ToList()
            };
        }
    }
}