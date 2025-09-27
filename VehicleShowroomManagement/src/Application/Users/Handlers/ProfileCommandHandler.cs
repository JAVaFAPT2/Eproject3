using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Users.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Users.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Users.Handlers
{
    /// <summary>
    /// Handler for updating employee profile
    /// </summary>
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
    {
        private readonly IRepository<Employee> _employeeRepository;

        public UpdateProfileCommandHandler(
            IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            // Update profile information
            if (!string.IsNullOrEmpty(request.Name))
            {
                employee.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Position))
            {
                employee.Position = request.Position;
            }

            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for changing employee status
    /// </summary>
    public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, Unit>
    {
        private readonly IRepository<Employee> _employeeRepository;

        public ChangeStatusCommandHandler(
            IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            // Update status
            employee.Status = request.Status;
            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return Unit.Value;
        }

    }

    /// <summary>
    /// Handler for updating an employee (admin function)
    /// </summary>
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        private readonly IRepository<Employee> _employeeRepository;

        public UpdateEmployeeCommandHandler(
            IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null || employee.IsDeleted)
            {
                throw new ArgumentException("Employee not found");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(request.Name))
            {
                employee.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Position))
            {
                employee.Position = request.Position;
            }

            if (!string.IsNullOrEmpty(request.Role))
            {
                employee.Role = request.Role;
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                employee.Status = request.Status;
            }

            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for deleting an employee (admin function)
    /// </summary>
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IRepository<Employee> _employeeRepository;

        public DeleteEmployeeCommandHandler(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            // Soft delete the employee
            employee.SoftDelete();

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for getting employee profile
    /// </summary>
    public class GetEmployeeProfileQueryHandler : IRequestHandler<GetEmployeeProfileQuery, EmployeeProfileDto>
    {
        private readonly IRepository<Employee> _employeeRepository;

        public GetEmployeeProfileQueryHandler(
            IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeProfileDto> Handle(GetEmployeeProfileQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
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
    }
}
