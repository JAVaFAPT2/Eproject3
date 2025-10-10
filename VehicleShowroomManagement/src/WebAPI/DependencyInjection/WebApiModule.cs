using Autofac;

namespace VehicleShowroomManagement.WebAPI.DependencyInjection
{
    /// <summary>
    /// Autofac module for WebAPI layer dependencies
    /// </summary>
    public class WebApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register any WebAPI-specific services here
            // Controllers are registered automatically by ASP.NET Core
        }
    }
}

