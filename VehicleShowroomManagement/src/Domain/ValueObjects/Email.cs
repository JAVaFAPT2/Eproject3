using System.Text.RegularExpressions;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Email value object with validation
    /// </summary>
    public record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be null or empty", nameof(value));

            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format", nameof(value));

            Value = value.ToLowerInvariant();
        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static implicit operator string(Email email) => email.Value;
        
        public static implicit operator Email(string email) => new(email);

        public override string ToString() => Value;
    }
}