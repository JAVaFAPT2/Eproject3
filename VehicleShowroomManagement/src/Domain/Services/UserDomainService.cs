using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Implementation of user domain service
    /// </summary>
    public class UserDomainService : IUserDomainService
    {
        private readonly IPasswordService _passwordService;

        public UserDomainService(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public Task<User> CreateUserAsync(
            string username,
            string email,
            string password,
            string firstName,
            string lastName,
            string roleId)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required", nameof(password));
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required", nameof(lastName));
            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentException("Role ID is required", nameof(roleId));

            // Business rules validation
            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long", nameof(password));

            // Create user entity
            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = username,
                Email = email.ToLowerInvariant(),
                PasswordHash = _passwordService.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                RoleId = roleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            return Task.FromResult(user);
        }

        public Task UpdateUserProfileAsync(
            User user,
            string? firstName,
            string? lastName,
            string? email,
            string? phone,
            decimal? salary,
            string? roleId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Apply updates with business rules
            if (!string.IsNullOrWhiteSpace(firstName))
                user.FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName))
                user.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(phone))
                user.Phone = phone;
            if (salary.HasValue)
            {
                if (salary.Value < 0)
                    throw new ArgumentException("Salary cannot be negative", nameof(salary));
                user.Salary = salary.Value;
            }
            if (!string.IsNullOrWhiteSpace(roleId))
                user.RoleId = roleId;

            user.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public Task<bool> ValidateCredentialsAsync(User user, string password)
        {
            if (user == null)
                return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(password))
                return Task.FromResult(false);

            return Task.FromResult(_passwordService.VerifyPassword(password, user.PasswordHash));
        }

        public Task ChangePasswordAsync(User user, string newPassword)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required", nameof(newPassword));

            if (newPassword.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long", nameof(newPassword));

            user.PasswordHash = _passwordService.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.SoftDelete();
            return Task.CompletedTask;
        }

        public Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);
            user.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(token);
        }

        public Task<bool> ValidatePasswordResetTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(false);

            // This validation will be done by the application layer
            // Domain service just defines the business rules
            return Task.FromResult(true);
        }

        public Task ResetPasswordWithTokenAsync(string token, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is required", nameof(token));
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required", nameof(newPassword));

            if (newPassword.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long", nameof(newPassword));

            // The actual token validation and user lookup will be done by application layer
            // Domain service defines the business logic for password reset

            return Task.CompletedTask;
        }
    }
}
