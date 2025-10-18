using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.Infrastructure.Repositories;
using VehicleShowroomManagement.Infrastructure.Services;
using VehicleShowroomManagement.Infrastructure.Resilience;
using InfrastructurePasswordService = VehicleShowroomManagement.Infrastructure.Services.PasswordService;

namespace VehicleShowroomManagement.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Autofac module for Infrastructure layer dependencies
    /// </summary>
    public class InfrastructureModule(IConfiguration configuration) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Configure MongoDB class maps (must be done before any MongoDB operations)
            MongoDbClassMapConfiguration.Configure();

            // MongoDB Configuration
            var connectionString = configuration.GetConnectionString("MongoDB");
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("VehicleShowroomDB");

            builder.RegisterInstance(mongoClient).As<IMongoClient>().SingleInstance();
            builder.RegisterInstance(database).As<IMongoDatabase>().SingleInstance();
            builder.RegisterType<VehicleShowroomDbContext>().AsSelf().InstancePerLifetimeScope();

            // Specific Repository Registrations with collection names
            builder.Register(c => new MongoRepository<Role>(c.Resolve<VehicleShowroomDbContext>(), "ROLE"))
                   .As<IRepository<Role>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<User>(c.Resolve<VehicleShowroomDbContext>(), "USER"))
                   .As<IRepository<User>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<VehicleModel>(c.Resolve<VehicleShowroomDbContext>(), "VEHICLE_MODEL"))
                   .As<IRepository<VehicleModel>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<Vehicle>(c.Resolve<VehicleShowroomDbContext>(), "VEHICLE"))
                   .As<IRepository<Vehicle>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<VehiclePhoto>(c.Resolve<VehicleShowroomDbContext>(), "VEHICLE_PHOTO"))
                   .As<IRepository<VehiclePhoto>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<VehicleSpec>(c.Resolve<VehicleShowroomDbContext>(), "VEHICLE_SPEC"))
                   .As<IRepository<VehicleSpec>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<PurchaseOrder>(c.Resolve<VehicleShowroomDbContext>(), "PURCHASE_ORDER"))
                   .As<IRepository<PurchaseOrder>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<PurchaseOrderLine>(c.Resolve<VehicleShowroomDbContext>(), "PURCHASE_ORDER_LINE"))
                   .As<IRepository<PurchaseOrderLine>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<Order>(c.Resolve<VehicleShowroomDbContext>(), "ORDER"))
                   .As<IRepository<Order>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<ServiceOrder>(c.Resolve<VehicleShowroomDbContext>(), "SERVICE_ORDER"))
                   .As<IRepository<ServiceOrder>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<BillingDocument>(c.Resolve<VehicleShowroomDbContext>(), "BILLING_DOCUMENT"))
                   .As<IRepository<BillingDocument>>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new MongoRepository<DocumentOutput>(c.Resolve<VehicleShowroomDbContext>(), "DOCUMENT_OUTPUT"))
                   .As<IRepository<DocumentOutput>>()
                   .InstancePerLifetimeScope();

            // Unit of Work
            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

            // Domain Services
            builder.RegisterType<InfrastructurePasswordService>()
                   .As<IPasswordService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<PricingService>()
                   .As<IPricingService>()
                   .InstancePerLifetimeScope();

            // Infrastructure Services
            builder.RegisterType<EmailService>()
                   .As<IEmailService>()
                   .WithParameter("resiliencePolicy", c => c.ResolveNamed<AsyncPolicy>("EmailPolicy"))
                   .InstancePerLifetimeScope();

            builder.RegisterType<CloudinaryService>()
                   .As<ICloudinaryService>()
                   .WithParameter("resiliencePolicy", c => c.ResolveNamed<AsyncPolicy>("CloudinaryPolicy"))
                   .InstancePerLifetimeScope();

            builder.RegisterType<PdfService>()
                   .As<IPdfService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ExcelService>()
                   .As<IExcelService>()
                   .InstancePerLifetimeScope();

            // MongoDB Index Initializer
            builder.RegisterType<MongoDbIndexInitializer>()
                   .AsSelf()
                   .SingleInstance();

            // Resilience Policies
            builder.RegisterInstance(ResiliencePolicies.GetCloudinaryPolicy())
                   .Named<AsyncPolicy>("CloudinaryPolicy")
                   .SingleInstance();

            builder.RegisterInstance(ResiliencePolicies.GetEmailPolicy())
                   .Named<AsyncPolicy>("EmailPolicy")
                   .SingleInstance();
        }
    }
}
