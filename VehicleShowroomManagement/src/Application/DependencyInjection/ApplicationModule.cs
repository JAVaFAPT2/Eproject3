using Autofac;
using MediatR;
using System.Reflection;
using AutofacModule = Autofac.Module;

namespace VehicleShowroomManagement.Application.DependencyInjection
{
    /// <summary>
    /// Autofac module for Application layer dependencies
    /// </summary>
    public class ApplicationModule : AutofacModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register MediatR and all command/query handlers from this assembly
            var applicationAssembly = Assembly.GetExecutingAssembly();
            
            // Register MediatR
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // Register all handlers
            builder.RegisterAssemblyTypes(applicationAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(applicationAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(applicationAssembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();

            // Register pipeline behaviors
            builder.RegisterAssemblyTypes(applicationAssembly)
                .AsClosedTypesOf(typeof(IPipelineBehavior<,>))
                .AsImplementedInterfaces();

            // Domain services registration (if any in Application layer)
            // Add any additional application-specific services here
        }
    }
}

