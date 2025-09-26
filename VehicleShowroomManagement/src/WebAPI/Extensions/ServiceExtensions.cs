using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Handlers;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Interfaces;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.Infrastructure.Repositories;

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
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

        /// <summary>
        /// Configures infrastructure services
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register MongoDB
            services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MongoDB");
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var configuration = sp.GetRequiredService<IConfiguration>();
                var databaseName = configuration["MongoDB:DatabaseName"] ?? "VehicleShowroomDB";
                return client.GetDatabase(databaseName);
            });

            // Register database context
            services.AddScoped<VehicleShowroomDbContext>();

            // Register repositories with MongoDB implementation
            services.AddScoped<IRepository<User>, MongoRepository<User>>();
            services.AddScoped<IRepository<Role>, MongoRepository<Role>>();
            services.AddScoped<IRepository<UserRole>, MongoRepository<UserRole>>();
            services.AddScoped<IRepository<Brand>, MongoRepository<Brand>>();
            services.AddScoped<IRepository<Model>, MongoRepository<Model>>();
            services.AddScoped<IRepository<Vehicle>, MongoRepository<Vehicle>>();
            services.AddScoped<IRepository<VehicleImage>, MongoRepository<VehicleImage>>();
            services.AddScoped<IRepository<Customer>, MongoRepository<Customer>>();
            services.AddScoped<IRepository<Supplier>, MongoRepository<Supplier>>();
            services.AddScoped<IRepository<PurchaseOrder>, MongoRepository<PurchaseOrder>>();
            services.AddScoped<IRepository<PurchaseOrderItem>, MongoRepository<PurchaseOrderItem>>();
            services.AddScoped<IRepository<SalesOrder>, MongoRepository<SalesOrder>>();
            services.AddScoped<IRepository<SalesOrderItem>, MongoRepository<SalesOrderItem>>();
            services.AddScoped<IRepository<Invoice>, MongoRepository<Invoice>>();
            services.AddScoped<IRepository<Payment>, MongoRepository<Payment>>();
            services.AddScoped<IRepository<ServiceOrder>, MongoRepository<ServiceOrder>>();
            services.AddScoped<IRepository<ServiceOrderItem>, MongoRepository<ServiceOrderItem>>();

            // Register domain services
            services.AddScoped<IPricingService, PricingService>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}