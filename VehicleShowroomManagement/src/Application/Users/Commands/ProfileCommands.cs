using System;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Commands
{
    /// <summary>
    /// Command for updating employee profile
    /// </summary>
    public class UpdateProfileCommand : IRequest<Unit>
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Position { get; set; }

        public UpdateProfileCommand(string employeeId, string? name, string? position)
        {
            EmployeeId = employeeId;
            Name = name;
            Position = position;
        }
    }

    /// <summary>
    /// Command for changing employee status
    /// </summary>
    public class ChangeStatusCommand : IRequest<Unit>
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public ChangeStatusCommand(string employeeId, string status)
        {
            EmployeeId = employeeId;
            Status = status;
        }
    }

    /// <summary>
    /// Command for updating an employee (admin function)
    /// </summary>
    public class UpdateEmployeeCommand : IRequest<Unit>
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }

        public UpdateEmployeeCommand(
            string employeeId,
            string? name = null,
            string? position = null,
            string? role = null,
            string? status = null)
        {
            EmployeeId = employeeId;
            Name = name;
            Position = position;
            Role = role;
            Status = status;
        }
    }

    /// <summary>
    /// Command for deleting an employee (admin function)
    /// </summary>
    public class DeleteEmployeeCommand : IRequest<Unit>
    {
        public string EmployeeId { get; set; } = string.Empty;

        public DeleteEmployeeCommand(string employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
