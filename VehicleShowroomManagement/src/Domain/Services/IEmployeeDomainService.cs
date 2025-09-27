using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Domain service for employee management business logic
    /// </summary>
    public interface IEmployeeDomainService : IDomainService
    {
        /// <summary>
        /// Creates a new employee with validation and business rules
        /// </summary>
        Task<Employee> CreateEmployeeAsync(
            string employeeId,
            string name,
            string role,
            string? position,
            DateTime hireDate);

        /// <summary>
        /// Updates employee profile information
        /// </summary>
        Task UpdateEmployeeProfileAsync(
            Employee employee,
            string? name,
            string? position,
            string? status);

        /// <summary>
        /// Changes employee status
        /// </summary>
        Task ChangeEmployeeStatusAsync(Employee employee, string status);

        /// <summary>
        /// Soft deletes an employee
        /// </summary>
        Task DeleteEmployeeAsync(Employee employee);
    }
}

