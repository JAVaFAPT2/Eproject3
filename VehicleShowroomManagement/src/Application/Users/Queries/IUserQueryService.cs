using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Query service for employee read operations
    /// </summary>
    public interface IUserQueryService
    {
        /// <summary>
        /// Gets all employees with optional filtering
        /// </summary>
        Task<IEnumerable<EmployeeDto>> GetUsersAsync(
            string? searchTerm = null,
            int? roleId = null,
            bool? isActive = null,
            int pageNumber = 1,
            int pageSize = 10);

        /// <summary>
        /// Gets an employee by ID
        /// </summary>
        Task<EmployeeDto?> GetUserByIdAsync(string userId);

        /// <summary>
        /// Gets current employee profile
        /// </summary>
        Task<EmployeeProfileDto?> GetUserProfileAsync(string userId);

        /// <summary>
        /// Gets employees with filtering
        /// </summary>
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10);
    }
}

