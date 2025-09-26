using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Query service for user read operations
    /// </summary>
    public interface IUserQueryService
    {
        /// <summary>
        /// Gets all users with optional filtering
        /// </summary>
        Task<IEnumerable<UserDto>> GetUsersAsync(
            string? searchTerm = null,
            int? roleId = null,
            bool? isActive = null,
            int pageNumber = 1,
            int pageSize = 10);

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        Task<UserDto?> GetUserByIdAsync(string userId);

        /// <summary>
        /// Gets current user profile
        /// </summary>
        Task<UserProfileDto?> GetUserProfileAsync(string userId);

        /// <summary>
        /// Gets employees with filtering
        /// </summary>
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10);
    }
}

