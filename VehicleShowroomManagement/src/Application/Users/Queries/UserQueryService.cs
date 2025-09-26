using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Implementation of user query service
    /// </summary>
    public class UserQueryService : IUserQueryService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public UserQueryService(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(
            string? searchTerm = null,
            int? roleId = null,
            bool? isActive = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var users = allUsers.Where(u => !u.IsDeleted).AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                users = users.Where(u =>
                    u.FirstName.ToLower().Contains(term) ||
                    u.LastName.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term) ||
                    u.Username.ToLower().Contains(term));
            }

            if (roleId.HasValue)
            {
                users = users.Where(u => u.RoleId == roleId.Value.ToString());
            }

            if (isActive.HasValue)
            {
                users = users.Where(u => u.IsActive == isActive.Value);
            }

            // Apply pagination
            var skip = (pageNumber - 1) * pageSize;
            var paginatedUsers = users.Skip(skip).Take(pageSize);

            var userDtos = new List<UserDto>();
            foreach (var user in paginatedUsers)
            {
                var role = await _roleRepository.GetByIdAsync(user.RoleId);
                userDtos.Add(new UserDto
                {
                    UserId = int.Parse(user.Id),
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = int.Parse(user.RoleId),
                    RoleName = role?.RoleName ?? "Unknown",
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                });
            }

            return userDtos;
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return null;
            }

            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            return new UserDto
            {
                UserId = int.Parse(user.Id),
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = int.Parse(user.RoleId),
                RoleName = role?.RoleName ?? "Unknown",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return null;
            }

            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            return new UserProfileDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = role?.RoleName ?? "Unknown",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var users = allUsers.Where(u => !u.IsDeleted && u.IsActive).ToList();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                users = users.Where(u =>
                    u.FirstName.ToLower().Contains(term) ||
                    u.LastName.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term) ||
                    u.Username.ToLower().Contains(term)).ToList();
            }

            // Apply pagination
            var skip = (pageNumber - 1) * pageSize;
            var paginatedUsers = users.Skip(skip).Take(pageSize);

            var employeeDtos = new List<EmployeeDto>();
            foreach (var user in paginatedUsers)
            {
                var role = await _roleRepository.GetByIdAsync(user.RoleId);
                employeeDtos.Add(new EmployeeDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Salary = user.Salary,
                    RoleId = user.RoleId,
                    RoleName = role?.RoleName ?? "Unknown",
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    TotalSales = 0, // Would need to calculate from orders
                    TotalRevenue = 0 // Would need to calculate from orders
                });
            }

            return employeeDtos;
        }
    }
}

