using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using BCrypt.Net;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Database seed data for initial setup
    /// </summary>
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var employeeRepository = services.GetRequiredService<IRepository<Employee>>();

            // Check if data already exists
            var existingEmployees = await employeeRepository.CountAsync(e => !e.IsDeleted);
            if (existingEmployees > 0)
            {
                return; // Data already seeded
            }

            // Seed default admin employee using proper constructor
            var adminEmployee = new Employee(
                employeeId: "ADMIN001",
                name: "System Administrator", 
                role: UserRole.Admin,
                position: "System Administrator",
                hireDate: DateTime.UtcNow,
                salary: 100000m);

            await employeeRepository.AddAsync(adminEmployee);

            // Seed default HR employee
            var hrEmployee = new Employee(
                employeeId: "HR001",
                name: "HR Manager",
                role: UserRole.HR,
                position: "Human Resources Manager",
                hireDate: DateTime.UtcNow,
                salary: 75000m);

            await employeeRepository.AddAsync(hrEmployee);

            // Seed default Dealer employee
            var dealerEmployee = new Employee(
                employeeId: "DEALER001",
                name: "Sales Manager",
                role: UserRole.Dealer,
                position: "Senior Sales Representative",
                hireDate: DateTime.UtcNow,
                salary: 65000m);

            await employeeRepository.AddAsync(dealerEmployee);

            Console.WriteLine("MongoDB database seeded successfully!");
        }

        private static string HashPassword(string password)
        {
            // Use BCrypt for secure password hashing
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }
    }
}