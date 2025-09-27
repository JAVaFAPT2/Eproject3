using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.Infrastructure.Repositories;
using InfrastructurePasswordService = VehicleShowroomManagement.Infrastructure.Services.PasswordService;

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
            services.AddScoped<VehicleShowroomDbContext>();

            // Repository Registration
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IRepository<Vehicle>>(provider => new MongoRepository<Vehicle>(provider.GetRequiredService<VehicleShowroomDbContext>(), "vehicles"));
            services.AddScoped<IRepository<Customer>>(provider => new MongoRepository<Customer>(provider.GetRequiredService<VehicleShowroomDbContext>(), "customers"));
            services.AddScoped<IRepository<Employee>>(provider => new MongoRepository<Employee>(provider.GetRequiredService<VehicleShowroomDbContext>(), "employees"));
            services.AddScoped<IRepository<SalesOrder>>(provider => new MongoRepository<SalesOrder>(provider.GetRequiredService<VehicleShowroomDbContext>(), "salesorders"));
            // TODO: Add other repositories as they are implemented

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Domain Services
            services.AddScoped<IPasswordService, InfrastructurePasswordService>();
            // TODO: Add other domain services as they are implemented

            return services;
        }
    }
}