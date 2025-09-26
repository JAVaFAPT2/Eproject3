using System;
using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Commands
{
    /// <summary>
    /// Command for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommand : IRequest<VehicleDto>
    {
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ExternalId { get; set; }

        public CreateVehicleCommand(
            string modelNumber,
            string name,
            string brand,
            decimal price,
            string status,
            string registrationNumber,
            DateTime registrationDate,
            string externalId)
        {
            ModelNumber = modelNumber;
            Name = name;
            Brand = brand;
            Price = price;
            Status = status;
            RegistrationNumber = registrationNumber;
            RegistrationDate = registrationDate;
            ExternalId = externalId;
        }
    }

    /// <summary>
    /// Command for updating a vehicle
    /// </summary>
    public class UpdateVehicleCommand : IRequest<Unit>
    {
        public string VehicleId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }

        public UpdateVehicleCommand(string vehicleId, decimal price, string status)
        {
            VehicleId = vehicleId;
            Price = price;
            Status = status;
        }
    }

    /// <summary>
    /// Command for deleting a vehicle (soft delete)
    /// </summary>
    public class DeleteVehicleCommand : IRequest<Unit>
    {
        public string VehicleId { get; set; }

        public DeleteVehicleCommand(string vehicleId)
        {
            VehicleId = vehicleId;
        }
    }

    /// <summary>
    /// Command for deleting multiple vehicles (soft delete)
    /// </summary>
    public class DeleteVehiclesCommand : IRequest<Unit>
    {
        public List<string> VehicleIds { get; set; }

        public DeleteVehiclesCommand(List<string> vehicleIds)
        {
            VehicleIds = vehicleIds;
        }
    }
}