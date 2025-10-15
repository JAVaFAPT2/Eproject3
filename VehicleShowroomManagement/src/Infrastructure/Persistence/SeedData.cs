using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VehicleShowroomManagement.Domain.Entities;

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

            // Ensure required roles exist (idempotent)
            var requiredRoleNames = new[] { "Admin", "HR", "Dealer", "Customer" };
            foreach (var roleName in requiredRoleNames)
            {
                var exists = await context.Roles
                    .Find(r => r.Name == roleName)
                    .AnyAsync();

                if (!exists)
                {
                    await context.Roles.InsertOneAsync(new Role(roleName));
                    Console.WriteLine($"✅ Seeded role: {roleName}");
                }
            }

            // Resolve Admin role for default admin user
            var adminRoleId = await context.Roles
                .Find(r => r.Name == "Admin")
                .Project(r => r.Id)
                .FirstOrDefaultAsync();

            // Seed default admin user if not exists (idempotent)
            var adminExists = await context.Users
                .Find(u => u.Username == "admin")
                .AnyAsync();

            if (!adminExists && !string.IsNullOrWhiteSpace(adminRoleId))
            {
                var passwordService = services.GetRequiredService<Domain.Services.IPasswordService>();
                var adminPasswordHash = passwordService.HashPassword("Admin123!");

                var adminUser = new User(
                    username: "admin",
                    passwordHash: adminPasswordHash,
                    name: "System Administrator",
                    email: "admin@vehicleshowroom.com",
                    roleId: adminRoleId,
                    phone: "+1234567890",
                    address: "123 Admin Street, City, Country",
                    hireDate: DateTime.UtcNow
                );

                await context.Users.InsertOneAsync(adminUser);
                Console.WriteLine("✅ Seeded admin user (username: admin, password: Admin123!)");
            }

            Console.WriteLine("MongoDB database seed check completed.");
        }
    }
}
