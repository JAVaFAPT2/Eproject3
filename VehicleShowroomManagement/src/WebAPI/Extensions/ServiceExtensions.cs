using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Register domain services
            services.AddScoped<IPricingService, PricingService>();

            // Register database context
            services.AddScoped<VehicleShowroomDbContext>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}