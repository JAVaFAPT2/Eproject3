using System;
using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Implementation of employee domain service
    /// </summary>
    public class EmployeeDomainService : IEmployeeDomainService
    {
        public Task<Employee> CreateEmployeeAsync(
            string employeeId,
            string name,
            string role,
            string? position,
            DateTime hireDate)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID is required", nameof(employeeId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required", nameof(role));

            // Business rules validation
            if (!new[] { "Dealer", "HR", "Admin" }.Contains(role))
                throw new ArgumentException("Invalid role specified", nameof(role));

            // Create employee entity
            var employee = new Employee
            {
                Id = Guid.NewGuid().ToString(),
                EmployeeId = employeeId,
                Name = name,
                Role = role,
                Position = position,
                HireDate = hireDate,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            return Task.FromResult(employee);
        }

        public Task UpdateEmployeeProfileAsync(
            Employee employee,
            string? name,
            string? position,
            string? status)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            // Apply updates with business rules
            if (!string.IsNullOrWhiteSpace(name))
                employee.Name = name;
            if (!string.IsNullOrWhiteSpace(position))
                employee.Position = position;
            if (!string.IsNullOrWhiteSpace(status))
                employee.Status = status;

            employee.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public Task ChangeEmployeeStatusAsync(Employee employee, string status)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status is required", nameof(status));

            employee.Status = status;
            employee.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public Task DeleteEmployeeAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            employee.SoftDelete();
            return Task.CompletedTask;
        }
    }
}

