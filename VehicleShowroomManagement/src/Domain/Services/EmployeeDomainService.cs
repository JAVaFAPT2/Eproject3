using System;
using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

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
            Enums.UserRole role,
            string? position,
            DateTime hireDate)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID is required", nameof(employeeId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            // Create employee entity using the new constructor
            var employee = new Employee(employeeId, name, role, position, hireDate);

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

            // Apply updates using domain methods
            if (!string.IsNullOrWhiteSpace(name))
                employee.UpdateProfile(name, position);
            else if (!string.IsNullOrWhiteSpace(position))
                employee.UpdateProfile(employee.Name, position);

            if (!string.IsNullOrWhiteSpace(status))
                employee.ChangeStatus(status);

            return Task.CompletedTask;
        }

        public Task ChangeEmployeeStatusAsync(Employee employee, string status)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status is required", nameof(status));

            employee.ChangeStatus(status);
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

