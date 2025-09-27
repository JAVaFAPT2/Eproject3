using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Bson;
using VehicleShowroomManagement.Application.Vehicles.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleEntity = VehicleShowroomManagement.Domain.Entities.Vehicle;
using BrandEntity = VehicleShowroomManagement.Domain.Entities.Brand;
using VehicleModelEntity = VehicleShowroomManagement.Domain.Entities.VehicleModel;


namespace VehicleShowroomManagement.Application.Vehicles.Handlers
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
    {
        private readonly IRepository<VehicleEntity> _vehicleRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<VehicleModelEntity> _vehicleModelRepository;

        public CreateVehicleCommandHandler(
            IRepository<VehicleEntity> vehicleRepository,
            IRepository<Brand> brandRepository,
            IRepository<VehicleModelEntity> vehicleModelRepository)
        {
            _vehicleRepository = vehicleRepository;
            _brandRepository = brandRepository;
            _vehicleModelRepository = vehicleModelRepository;
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

            // Check if vehicle model exists, create if not
            var vehicleModel = await _vehicleModelRepository.FirstOrDefaultAsync(m =>
                m.Name == request.Name &&
                m.Brand == brand.BrandName &&
                !m.IsDeleted);

            if (vehicleModel == null)
            {
                vehicleModel = new VehicleModelEntity
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    ModelNumber = request.ModelNumber,
                    Name = request.Name,
                    Brand = brand.BrandName,
                    BasePrice = request.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await _vehicleModelRepository.AddAsync(vehicleModel);
            }

            // Create vehicle
            var vehicle = new VehicleEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                VehicleId = request.VehicleId ?? $"VEH-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}",
                ModelNumber = vehicleModel.ModelNumber,
                ExternalNumber = request.ExternalId,
                RegistrationData = new RegistrationData
                {
                    VIN = request.ModelNumber, // Using ModelNumber as VIN for now
                    LicensePlate = request.RegistrationNumber ?? "TEMP-001",
                    RegistrationDate = request.RegistrationDate,
                    ExpiryDate = request.RegistrationDate.AddYears(1)
                },
                Status = request.Status,
                PurchasePrice = request.Price,
                Photos = new List<string>(),
                ReceiptDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
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
                VehicleId = vehicle.VehicleId,
                VIN = vehicle.RegistrationData?.VIN ?? string.Empty,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                Name = vehicle.ModelNumber, // Using ModelNumber as name
                Brand = "Unknown", // Not available in new schema
                BrandId = string.Empty,
                ModelId = vehicle.ModelNumber,
                Color = "Unknown", // Not available in new schema
                Year = 0, // Not available in new schema
                PurchasePrice = vehicle.PurchasePrice,
                Mileage = 0, // Not available in new schema
                Status = vehicle.Status,
                LicensePlate = vehicle.RegistrationData?.LicensePlate ?? string.Empty,
                RegistrationDate = vehicle.RegistrationData?.RegistrationDate,
                ExpiryDate = vehicle.RegistrationData?.ExpiryDate,
                Photos = vehicle.Photos ?? new List<string>(),
                ReceiptDate = vehicle.ReceiptDate,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                Images = new List<VehicleImageDto>() // Not available in new schema
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

            vehicle.PurchasePrice = request.PurchasePrice;

            if (!string.IsNullOrEmpty(request.Status))
            {
                vehicle.Status = request.Status;
            }

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
