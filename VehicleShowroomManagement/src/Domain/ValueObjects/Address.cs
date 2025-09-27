using System.Text.RegularExpressions;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Address value object with validation
    /// </summary>
    public record Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }

        public Address(string street, string city, string state, string zipCode, string country = "USA")
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be null or empty", nameof(street));
            
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be null or empty", nameof(city));
            
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("State cannot be null or empty", nameof(state));
            
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentException("Zip code cannot be null or empty", nameof(zipCode));

            if (!IsValidZipCode(zipCode))
                throw new ArgumentException("Invalid zip code format", nameof(zipCode));

            Street = street.Trim();
            City = city.Trim();
            State = state.Trim().ToUpperInvariant();
            ZipCode = zipCode.Trim();
            Country = string.IsNullOrWhiteSpace(country) ? "USA" : country.Trim();
        }

        private static bool IsValidZipCode(string zipCode)
        {
            // US ZIP code pattern: 12345 or 12345-6789
            var zipRegex = new Regex(@"^\d{5}(-\d{4})?$");
            return zipRegex.IsMatch(zipCode);
        }

        public override string ToString()
        {
            return $"{Street}, {City}, {State} {ZipCode}, {Country}";
        }
    }
}