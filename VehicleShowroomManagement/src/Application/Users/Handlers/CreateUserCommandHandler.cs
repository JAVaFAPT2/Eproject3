using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Users.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Users.Handlers
{
    /// <summary>
    /// Handler for creating a new employee
    /// </summary>
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, string>
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IEmployeeDomainService _employeeDomainService;

        public CreateEmployeeCommandHandler(
            IRepository<Employee> employeeRepository,
            IEmployeeDomainService employeeDomainService)
        {
            _employeeRepository = employeeRepository;
            _employeeDomainService = employeeDomainService;
        }

        public async Task<string> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Check if employee ID already exists
            var existingEmployee = await _employeeRepository.FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId);
            if (existingEmployee != null)
            {
                throw new ArgumentException($"Employee ID '{request.EmployeeId}' already exists");
            }

            // Use domain service to create employee
            var employee = await _employeeDomainService.CreateEmployeeAsync(
                request.EmployeeId,
                request.Name,
                request.Role,
                request.Position,
                request.HireDate);

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return employee.Id;
        }

    }
}
