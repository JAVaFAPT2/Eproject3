using System.Text.RegularExpressions;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Phone number value object with validation
    /// </summary>
    public record PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be null or empty", nameof(value));

            if (!IsValidPhoneNumber(value))
                throw new ArgumentException("Invalid phone number format", nameof(value));

            Value = CleanPhoneNumber(value);
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Remove all non-digit characters for validation
            var cleaned = Regex.Replace(phoneNumber, @"[^\d]", "");
            return cleaned.Length >= 10 && cleaned.Length <= 15;
        }

        private static string CleanPhoneNumber(string phoneNumber)
        {
            // Remove all non-digit characters except +
            return Regex.Replace(phoneNumber, @"[^\d+]", "");
        }

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
        
        public static implicit operator PhoneNumber(string phoneNumber) => new(phoneNumber);

        public override string ToString() => Value;
    }
}