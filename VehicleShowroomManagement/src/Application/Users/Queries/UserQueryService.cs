using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Implementation of employee query service
    /// </summary>
    public class UserQueryService : IUserQueryService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public UserQueryService(
            IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetUsersAsync(
            string? searchTerm = null,
            int? roleId = null,
            bool? isActive = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var allEmployees = await _employeeRepository.GetAllAsync();
            var employees = allEmployees.Where(e => !e.IsDeleted).AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                employees = employees.Where(e =>
                    e.Name.ToLower().Contains(term) ||
                    e.EmployeeId.ToLower().Contains(term) ||
                    e.Role.ToLower().Contains(term));
            }

            if (isActive.HasValue)
            {
                employees = employees.Where(e => e.IsActive == isActive.Value);
            }

            // Apply pagination
            var skip = (pageNumber - 1) * pageSize;
            var paginatedEmployees = employees.Skip(skip).Take(pageSize);

            var employeeDtos = new List<EmployeeDto>();
            foreach (var employee in paginatedEmployees)
            {
                employeeDtos.Add(new EmployeeDto
                {
                    Id = employee.Id,
                    EmployeeNumber = employee.EmployeeId,
                    Name = employee.Name,
                    Role = employee.Role,
                    Position = employee.Position,
                    HireDate = employee.HireDate,
                    Status = employee.Status,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt,
                    TotalSales = 0, // Would need to calculate from orders
                    TotalRevenue = 0 // Would need to calculate from orders
                });
            }

            return employeeDtos;
        }

        public async Task<EmployeeDto?> GetUserByIdAsync(string userId)
        {
            var employee = await _employeeRepository.GetByIdAsync(userId);
            if (employee == null || employee.IsDeleted)
            {
                return null;
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeId,
                Name = employee.Name,
                Role = employee.Role,
                Position = employee.Position,
                HireDate = employee.HireDate,
                Status = employee.Status,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
                TotalSales = 0, // Would need to calculate from orders
                TotalRevenue = 0 // Would need to calculate from orders
            };
        }

        public async Task<EmployeeProfileDto?> GetUserProfileAsync(string userId)
        {
            var employee = await _employeeRepository.GetByIdAsync(userId);
            if (employee == null || employee.IsDeleted)
            {
                return null;
            }

            return new EmployeeProfileDto
            {
                EmployeeId = employee.Id,
                EmployeeNumber = employee.EmployeeId,
                Name = employee.Name,
                Role = employee.Role,
                Position = employee.Position,
                HireDate = employee.HireDate,
                Status = employee.Status,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var allEmployees = await _employeeRepository.GetAllAsync();
            var employees = allEmployees.Where(e => !e.IsDeleted && e.IsActive).ToList();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                employees = employees.Where(e =>
                    e.Name.ToLower().Contains(term) ||
                    e.EmployeeId.ToLower().Contains(term) ||
                    e.Role.ToLower().Contains(term)).ToList();
            }

            // Apply pagination
            var skip = (pageNumber - 1) * pageSize;
            var paginatedEmployees = employees.Skip(skip).Take(pageSize);

            var employeeDtos = new List<EmployeeDto>();
            foreach (var employee in paginatedEmployees)
            {
                employeeDtos.Add(new EmployeeDto
                {
                    Id = employee.Id,
                    EmployeeNumber = employee.EmployeeId,
                    Name = employee.Name,
                    Role = employee.Role,
                    Position = employee.Position,
                    HireDate = employee.HireDate,
                    Status = employee.Status,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt,
                    TotalSales = 0, // Would need to calculate from orders
                    TotalRevenue = 0 // Would need to calculate from orders
                });
            }

            return employeeDtos;
        }
    }
}

