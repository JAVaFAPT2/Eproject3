using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.Infrastructure.Repositories;
using VehicleShowroomManagement.Infrastructure.Services;
using VehicleShowroomManagement.Application.Features.Auth.Commands.ForgotPassword;
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
            services.AddScoped<IRepository<PurchaseOrder>>(provider => new MongoRepository<PurchaseOrder>(provider.GetRequiredService<VehicleShowroomDbContext>(), "purchaseorders"));
            services.AddScoped<IRepository<GoodsReceipt>>(provider => new MongoRepository<GoodsReceipt>(provider.GetRequiredService<VehicleShowroomDbContext>(), "goodsreceipts"));
            services.AddScoped<IRepository<ServiceOrder>>(provider => new MongoRepository<ServiceOrder>(provider.GetRequiredService<VehicleShowroomDbContext>(), "serviceorders"));
            services.AddScoped<IRepository<VehicleRegistration>>(provider => new MongoRepository<VehicleRegistration>(provider.GetRequiredService<VehicleShowroomDbContext>(), "vehicleregistrations"));
            services.AddScoped<IRepository<ReturnRequest>>(provider => new MongoRepository<ReturnRequest>(provider.GetRequiredService<VehicleShowroomDbContext>(), "returnrequests"));
            services.AddScoped<IRepository<Invoice>>(provider => new MongoRepository<Invoice>(provider.GetRequiredService<VehicleShowroomDbContext>(), "invoices"));
            services.AddScoped<IRepository<Payment>>(provider => new MongoRepository<Payment>(provider.GetRequiredService<VehicleShowroomDbContext>(), "payments"));
            services.AddScoped<IRepository<VehicleImage>>(provider => new MongoRepository<VehicleImage>(provider.GetRequiredService<VehicleShowroomDbContext>(), "vehicleimages"));
            services.AddScoped<IRepository<RefreshToken>>(provider => new MongoRepository<RefreshToken>(provider.GetRequiredService<VehicleShowroomDbContext>(), "refreshtokens"));
            services.AddScoped<IRepository<Supplier>>(provider => new MongoRepository<Supplier>(provider.GetRequiredService<VehicleShowroomDbContext>(), "suppliers"));
            services.AddScoped<IRepository<Allotment>>(provider => new MongoRepository<Allotment>(provider.GetRequiredService<VehicleShowroomDbContext>(), "allotments"));
            services.AddScoped<IRepository<WaitingList>>(provider => new MongoRepository<WaitingList>(provider.GetRequiredService<VehicleShowroomDbContext>(), "waitinglist"));
            services.AddScoped<IRepository<Appointment>>(provider => new MongoRepository<Appointment>(provider.GetRequiredService<VehicleShowroomDbContext>(), "appointments"));
            services.AddScoped<IRepository<DealerAvailability>>(provider => new MongoRepository<DealerAvailability>(provider.GetRequiredService<VehicleShowroomDbContext>(), "dealeravailability"));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Domain Services
            services.AddScoped<IPasswordService, InfrastructurePasswordService>();

            // Infrastructure Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IExcelService, ExcelService>();

            return services;
        }
    }
}