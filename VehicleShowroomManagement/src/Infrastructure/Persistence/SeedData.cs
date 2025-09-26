using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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

            var userRepository = services.GetRequiredService<IRepository<User>>();
            var roleRepository = services.GetRequiredService<IRepository<Role>>();

            // Check if data already exists
            var existingRoles = await roleRepository.CountAsync(r => true);
            if (existingRoles > 0)
            {
                return; // Data already seeded
            }

            // Seed roles
            var hrRole = new Role
            {
                RoleName = "HR",
                Description = "Human Resources - manages employees and user accounts"
            };

            var dealerRole = new Role
            {
                RoleName = "Dealer",
                Description = "Vehicle Dealer - manages sales, inventory, and customer relations"
            };

            var adminRole = new Role
            {
                RoleName = "Admin",
                Description = "System Administrator - full system access"
            };

            await roleRepository.AddRangeAsync(new[] { hrRole, dealerRole, adminRole });
            await roleRepository.SaveChangesAsync();

            // Seed default admin user
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@vehicleshowroom.com",
                PasswordHash = HashPassword("Admin123!"),
                FirstName = "System",
                LastName = "Administrator",
                RoleId = adminRole.RoleId,
                IsActive = true
            };

            await userRepository.AddAsync(adminUser);
            await userRepository.SaveChangesAsync();

            Console.WriteLine("Database seeded successfully!");
        }

        private static string HashPassword(string password)
        {
            // In a real implementation, use BCrypt or similar
            return $"hashed_{password}";
        }
    }
}