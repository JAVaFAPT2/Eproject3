using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Vehicle entities
    /// </summary>
    public class VehicleDto
    {
        public string Id { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string BrandId { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string Status { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string ExternalId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<VehicleImageDto> Images { get; set; } = new List<VehicleImageDto>();
    }

    /// <summary>
    /// Data Transfer Object for Vehicle Images
    /// </summary>
    public class VehicleImageDto
    {
        public string ImageId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}