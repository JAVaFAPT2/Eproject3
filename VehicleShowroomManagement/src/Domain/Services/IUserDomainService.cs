using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Domain service for user management business logic
    /// </summary>
    public interface IUserDomainService : IDomainService
    {
        /// <summary>
        /// Creates a new user with validation and business rules
        /// </summary>
        Task<User> CreateUserAsync(
            string username,
            string email,
            string password,
            string firstName,
            string lastName,
            string roleId);

        /// <summary>
        /// Updates user profile information
        /// </summary>
        Task UpdateUserProfileAsync(
            User user,
            string? firstName,
            string? lastName,
            string? email,
            string? phone,
            decimal? salary,
            string? roleId);

        /// <summary>
        /// Validates user credentials
        /// </summary>
        Task<bool> ValidateCredentialsAsync(User user, string password);

        /// <summary>
        /// Changes user password
        /// </summary>
        Task ChangePasswordAsync(User user, string newPassword);

        /// <summary>
        /// Soft deletes a user
        /// </summary>
        Task DeleteUserAsync(User user);

        /// <summary>
        /// Generates password reset token
        /// </summary>
        Task<string> GeneratePasswordResetTokenAsync(User user);

        /// <summary>
        /// Validates and consumes password reset token
        /// </summary>
        Task<bool> ValidatePasswordResetTokenAsync(string token);

        /// <summary>
        /// Resets user password using token
        /// </summary>
        Task ResetPasswordWithTokenAsync(string token, string newPassword);
    }
}
