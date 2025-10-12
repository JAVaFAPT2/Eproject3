using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
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
            
            var configuration = MediatRConfigurationBuilder
                .Create("VehicleShowroomManagement.Application", applicationAssembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build();

            builder.RegisterMediatR(configuration);

            // Domain services registration (if any in Application layer)
            // Add any additional application-specific services here
        }
    }
}

