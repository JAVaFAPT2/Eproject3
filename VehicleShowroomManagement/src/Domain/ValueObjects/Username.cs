using System.Text.RegularExpressions;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Username value object with validation
    /// </summary>
    public record Username
    {
        public string Value { get; }

        public Username(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Username cannot be null or empty", nameof(value));

            if (!IsValidUsername(value))
                throw new ArgumentException("Invalid username format", nameof(value));

            Value = value.ToLowerInvariant();
        }

        private static bool IsValidUsername(string username)
        {
            // Username must be 3-30 characters, alphanumeric and underscores only
            var usernameRegex = new Regex(@"^[a-zA-Z0-9_]{3,30}$");
            return usernameRegex.IsMatch(username);
        }

        public static implicit operator string(Username username) => username.Value;
        
        public static implicit operator Username(string username) => new(username);

        public override string ToString() => Value;
    }
}