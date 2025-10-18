using Microsoft.Extensions.Logging;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Base service class with common logging functionality
    /// </summary>
    public abstract class BaseService
    {
        protected readonly ILogger Logger;

        protected BaseService(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Logs service operation start
        /// </summary>
        protected void LogOperationStart(string operationName, object? parameters = null)
        {
            Logger.LogInformation("Starting {OperationName} with parameters: {@Parameters}", 
                operationName, parameters);
        }

        /// <summary>
        /// Logs service operation completion
        /// </summary>
        protected void LogOperationComplete(string operationName, object? result = null)
        {
            Logger.LogInformation("Completed {OperationName} with result: {@Result}", 
                operationName, result);
        }

        /// <summary>
        /// Logs service operation error
        /// </summary>
        protected void LogOperationError(string operationName, Exception exception, object? parameters = null)
        {
            Logger.LogError(exception, "Error in {OperationName} with parameters: {@Parameters}", 
                operationName, parameters);
        }

        /// <summary>
        /// Logs service operation warning
        /// </summary>
        protected void LogOperationWarning(string operationName, string message, object? parameters = null)
        {
            Logger.LogWarning("Warning in {OperationName}: {Message} with parameters: {@Parameters}", 
                operationName, message, parameters);
        }
    }
}
