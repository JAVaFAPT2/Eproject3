using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using BCrypt.Net;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

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
            var roleRepository = services.GetRequiredService<IRepository<Role>>();

            // Check if data already exists
            var existingRoles = await roleRepository.CountAsync(r => !r.IsDeleted);
            if (existingRoles > 0)
            {
                return; // Data already seeded
            }

            // Seed roles
            var hrRole = new Role
            {
                Id = ObjectId.GenerateNewId().ToString(),
                RoleName = "HR",
                Description = "Human Resources - manages employees and user accounts",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var dealerRole = new Role
            {
                Id = ObjectId.GenerateNewId().ToString(),
                RoleName = "Dealer",
                Description = "Vehicle Dealer - manages sales, inventory, and customer relations",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var adminRole = new Role
            {
                Id = ObjectId.GenerateNewId().ToString(),
                RoleName = "Admin",
                Description = "System Administrator - full system access",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await roleRepository.AddRangeAsync(new[] { hrRole, dealerRole, adminRole });
            await roleRepository.SaveChangesAsync();

            // Seed default admin employee
            var adminEmployee = new Employee
            {
                Id = ObjectId.GenerateNewId().ToString(),
                EmployeeId = "ADMIN001",
                Name = "System Administrator",
                Role = "Admin",
                Position = "System Administrator",
                HireDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await employeeRepository.AddAsync(adminEmployee);
            await employeeRepository.SaveChangesAsync();

            Console.WriteLine("MongoDB database seeded successfully!");
        }

        private static string HashPassword(string password)
        {
            // Use BCrypt for secure password hashing
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }
    }
}