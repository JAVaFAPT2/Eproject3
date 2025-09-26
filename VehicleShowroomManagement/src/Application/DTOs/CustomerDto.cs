using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Customer entities
    /// </summary>
    public class CustomerDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string Cccd { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public string FullName => $"{FirstName} {LastName}";
        public AddressInfo? CustomerAddress => new AddressInfo(Address ?? "", City ?? "", State ?? "", ZipCode);
    }

    /// <summary>
    /// Embedded address information within Customer DTO
    /// </summary>
    public class AddressInfo
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string? ZipCode { get; set; }

        public AddressInfo(string street, string city, string state, string? zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string FullAddress => $"{Street}, {City}, {State} {(string.IsNullOrEmpty(ZipCode) ? "" : ZipCode)}";
    }
}