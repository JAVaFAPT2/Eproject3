using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.Infrastructure.Repositories;
using VehicleShowroomManagement.Infrastructure.Services;

namespace VehicleShowroomManagement.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods for configuring services
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configures application services
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register MediatR for CQRS
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Application.Features.Users.Commands.CreateUser.CreateUserCommand).Assembly);
            });

            return services;
        }

        /// <summary>
        /// Configures infrastructure services
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // MongoDB Configuration
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("VehicleShowroomDB");

            services.AddSingleton<IMongoDatabase>(database);
            services.AddSingleton<IMongoClient>(client);

            // Repository Registration
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            // TODO: Add other repositories as they are implemented

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Domain Services
            services.AddScoped<IPasswordService, PasswordService>();
            // TODO: Add other domain services as they are implemented

            return services;
        }
    }
}