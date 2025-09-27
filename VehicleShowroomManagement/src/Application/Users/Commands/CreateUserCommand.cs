using System;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Commands
{
    /// <summary>
    /// Command for creating a new employee
    /// </summary>
    public class CreateEmployeeCommand : IRequest<string>
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }

        public CreateEmployeeCommand(string employeeId, string name, string role, string position, DateTime hireDate)
        {
            EmployeeId = employeeId;
            Name = name;
            Role = role;
            Position = position;
            HireDate = hireDate;
        }
    }
}
