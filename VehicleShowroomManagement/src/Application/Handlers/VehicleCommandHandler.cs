using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Bson;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleEntity = VehicleShowroomManagement.Domain.Entities.Vehicle;
using BrandEntity = VehicleShowroomManagement.Domain.Entities.Brand;
using ModelEntity = VehicleShowroomManagement.Domain.Entities.Model;


namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
    {
        private readonly IRepository<VehicleEntity> _vehicleRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Model> _modelRepository;

        public CreateVehicleCommandHandler(
            IRepository<VehicleEntity> vehicleRepository,
            IRepository<Brand> brandRepository,
            IRepository<Model> modelRepository)
        {
            _vehicleRepository = vehicleRepository;
            _brandRepository = brandRepository;
            _modelRepository = modelRepository;
        }

        public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            // Check if brand exists, create if not
            var brand = await _brandRepository.FirstOrDefaultAsync(b =>
                b.BrandName == request.Brand && !b.IsDeleted);

            if (brand == null)
            {
                brand = new Brand
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    BrandName = request.Brand,
                    Country = "Vietnam", // Default country
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await _brandRepository.AddAsync(brand);
            }

            // Check if model exists, create if not
            var model = await _modelRepository.FirstOrDefaultAsync(m =>
                m.ModelName == request.Name &&
                m.BrandId == brand.Id &&
                !m.IsDeleted);

            if (model == null)
            {
                model = new Model
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    ModelName = request.Name,
                    BrandId = brand.Id,
                    EngineType = "Standard", // Default values
                    Transmission = "Manual",
                    FuelType = "Gasoline",
                    SeatingCapacity = 5,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await _modelRepository.AddAsync(model);
            }

            // Create vehicle
            var vehicle = new VehicleEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                VIN = request.ModelNumber, // Using ModelNumber as VIN for now
                ModelId = model.Id,
                Color = "Default", // Default color
                Year = request.RegistrationDate.Year,
                Price = request.Price,
                Mileage = 0,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
                // Model info will be populated via reference to ModelId
            };

            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return MapToDto(vehicle);
        }

        private static VehicleDto MapToDto(VehicleEntity vehicle)
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
    /// Handler for updating a vehicle
    /// </summary>
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Unit>
    {
        private readonly IRepository<VehicleEntity> _vehicleRepository;

        public UpdateVehicleCommandHandler(IRepository<VehicleEntity> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Unit> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found");
            }

            vehicle.Price = request.Price;
            vehicle.Status = request.Status;
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for deleting a vehicle (soft delete)
    /// </summary>
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Unit>
    {
        private readonly IRepository<VehicleEntity> _vehicleRepository;

        public DeleteVehicleCommandHandler(IRepository<VehicleEntity> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Unit> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found");
            }

            vehicle.SoftDelete();
            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for deleting multiple vehicles (soft delete)
    /// </summary>
    public class DeleteVehiclesCommandHandler : IRequestHandler<DeleteVehiclesCommand, Unit>
    {
        private readonly IRepository<VehicleEntity> _vehicleRepository;

        public DeleteVehiclesCommandHandler(IRepository<VehicleEntity> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Unit> Handle(DeleteVehiclesCommand request, CancellationToken cancellationToken)
        {
            foreach (var vehicleId in request.VehicleIds)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle != null)
                {
                    vehicle.SoftDelete();
                    await _vehicleRepository.UpdateAsync(vehicle);
                }
            }

            await _vehicleRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}