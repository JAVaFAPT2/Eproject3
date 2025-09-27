using System.Text.RegularExpressions;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Vehicle Identification Number (VIN) value object with validation
    /// </summary>
    public record Vin
    {
        public string Value { get; }

        public Vin(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("VIN cannot be null or empty", nameof(value));

            if (!IsValidVin(value))
                throw new ArgumentException("Invalid VIN format", nameof(value));

            Value = value.ToUpperInvariant();
        }

        private static bool IsValidVin(string vin)
        {
            // VIN must be exactly 17 characters and contain only valid characters
            var vinRegex = new Regex(@"^[A-HJ-NPR-Z0-9]{17}$");
            return vinRegex.IsMatch(vin);
        }

        public static implicit operator string(Vin vin) => vin.Value;
        
        public static implicit operator Vin(string vin) => new(vin);

        public override string ToString() => Value;
    }
}