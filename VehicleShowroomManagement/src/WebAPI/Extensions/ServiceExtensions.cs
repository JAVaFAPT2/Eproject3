using System.Reflection;
using CloudinaryDotNet;
using DocumentFormat.OpenXml.EMMA;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Images.Services;
using VehicleShowroomManagement.Application.Reports.Services;
using VehicleShowroomManagement.Application.Users.Queries;
using VehicleShowroomManagement.Application.Email.Services;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Interfaces;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.Infrastructure.Repositories;

using IPasswordService = VehicleShowroomManagement.Domain.Services.IPasswordService;

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
            services.AddScoped<IRepository<User>>(sp => new MongoRepository<User>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Users"));
            services.AddScoped<IRepository<Role>>(sp => new MongoRepository<Role>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Roles"));
            services.AddScoped<IRepository<UserRole>>(sp => new MongoRepository<UserRole>(sp.GetRequiredService<VehicleShowroomDbContext>(), "UserRoles"));
            services.AddScoped<IRepository<Brand>>(sp => new MongoRepository<Brand>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Brands"));
            services.AddScoped<IRepository<Model>>(sp => new MongoRepository<Model>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Models"));
            services.AddScoped<IRepository<Vehicle>>(sp => new MongoRepository<Vehicle>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Vehicles"));
            services.AddScoped<IRepository<VehicleImage>>(sp => new MongoRepository<VehicleImage>(sp.GetRequiredService<VehicleShowroomDbContext>(), "VehicleImages"));
            services.AddScoped<IRepository<Customer>>(sp => new MongoRepository<Customer>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Customers"));
            services.AddScoped<IRepository<SalesOrder>>(sp => new MongoRepository<SalesOrder>(sp.GetRequiredService<VehicleShowroomDbContext>(), "SalesOrders"));
            services.AddScoped<IRepository<SalesOrderItem>>(sp => new MongoRepository<SalesOrderItem>(sp.GetRequiredService<VehicleShowroomDbContext>(), "SalesOrderItems"));
            services.AddScoped<IRepository<Invoice>>(sp => new MongoRepository<Invoice>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Invoices"));
            services.AddScoped<IRepository<Payment>>(sp => new MongoRepository<Payment>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Payments"));
            services.AddScoped<IRepository<ServiceOrder>>(sp => new MongoRepository<ServiceOrder>(sp.GetRequiredService<VehicleShowroomDbContext>(), "ServiceOrders"));
            services.AddScoped<IRepository<ReturnRequest>>(sp => new MongoRepository<ReturnRequest>(sp.GetRequiredService<VehicleShowroomDbContext>(), "ReturnRequests"));
            services.AddScoped<IRepository<PurchaseOrder>>(sp => new MongoRepository<PurchaseOrder>(sp.GetRequiredService<VehicleShowroomDbContext>(), "PurchaseOrders"));
            services.AddScoped<IRepository<GoodsReceipt>>(sp => new MongoRepository<GoodsReceipt>(sp.GetRequiredService<VehicleShowroomDbContext>(), "GoodsReceipts"));
            services.AddScoped<IRepository<VehicleRegistration>>(sp => new MongoRepository<VehicleRegistration>(sp.GetRequiredService<VehicleShowroomDbContext>(), "VehicleRegistrations"));
            services.AddScoped<IRepository<Allotment>>(sp => new MongoRepository<Allotment>(sp.GetRequiredService<VehicleShowroomDbContext>(), "Allotments"));
            services.AddScoped<IRepository<WaitingList>>(sp => new MongoRepository<WaitingList>(sp.GetRequiredService<VehicleShowroomDbContext>(), "WaitingLists"));

            // Register domain services
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IUserDomainService, UserDomainService>();

            // Register query services
            services.AddScoped<IUserQueryService, UserQueryService>();

            // Register image upload service
            services.AddScoped<IImageUploadService, CloudinaryImageUploadService>();

            // Register Excel export service
            services.AddScoped<IExcelExportService, ExcelExportService>();

            // Register email services
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IEmailTemplateService, RazorEmailTemplateService>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}