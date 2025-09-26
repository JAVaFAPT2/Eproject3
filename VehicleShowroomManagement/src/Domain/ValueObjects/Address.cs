using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Address value object representing a physical address
    /// </summary>
    [ComplexType]
    public class Address : IEquatable<Address>
    {
        [Required]
        [StringLength(255)]
        public string Street { get; private set; }

        [Required]
        [StringLength(50)]
        public string City { get; private set; }

        [Required]
        [StringLength(50)]
        public string State { get; private set; }

        [StringLength(10)]
        public string? ZipCode { get; private set; }

        // Required for Entity Framework
        protected Address()
        {
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
        }

        public Address(string street, string city, string state, string? zipCode = null)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public override bool Equals(object? obj)
        {
            return obj is Address address && Equals(address);
        }

        public bool Equals(Address? other)
        {
            if (other is null)
                return false;

            return Street == other.Street &&
                   City == other.City &&
                   State == other.State &&
                   ZipCode == other.ZipCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, ZipCode);
        }

        public static bool operator ==(Address? left, Address? right)
        {
            return EqualityComparer<Address>.Default.Equals(left, right);
        }

        public static bool operator !=(Address? left, Address? right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            var address = $"{Street}, {City}, {State}";
            if (!string.IsNullOrEmpty(ZipCode))
                address += $" {ZipCode}";

            return address;
        }

        public Address Update(string street, string city, string state, string? zipCode = null)
        {
            return new Address(street, city, state, zipCode);
        }
    }
}