using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VehicleShowroomManagement.Domain.Entities;
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

            var context = services.GetRequiredService<VehicleShowroomDbContext>();
            
            // Check if roles already exist
            var existingRoles = await context.Roles.CountDocumentsAsync(_ => true);
            if (existingRoles > 0)
            {
                return; // Data already seeded
            }

            // Seed default roles
            var adminRole = new Role("Admin");
            var hrRole = new Role("HR");
            var dealerRole = new Role("Dealer");
            var customerRole = new Role("Customer");

            await context.Roles.InsertManyAsync(new[] { adminRole, hrRole, dealerRole, customerRole });

            Console.WriteLine("✅ Seeded roles: Admin, HR, Dealer, Customer");

            // Seed default admin user
            var passwordService = services.GetRequiredService<Domain.Services.IPasswordService>();
            var adminPasswordHash = passwordService.HashPassword("Admin123!");

            var adminUser = new User(
                username: "admin",
                passwordHash: adminPasswordHash,
                name: "System Administrator",
                email: "admin@vehicleshowroom.com",
                roleId: adminRole.Id,
                phone: "+1234567890",
                address: "123 Admin Street, City, Country",
                hireDate: DateTime.UtcNow
            );

            await context.Users.InsertOneAsync(adminUser);

            Console.WriteLine("✅ Seeded admin user (username: admin, password: Admin123!)");
            Console.WriteLine("MongoDB database seeded successfully!");
        }
    }
}
