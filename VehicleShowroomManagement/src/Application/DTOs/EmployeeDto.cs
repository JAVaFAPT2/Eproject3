using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Employee entities
    /// </summary>
    public class EmployeeDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public decimal Salary { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}