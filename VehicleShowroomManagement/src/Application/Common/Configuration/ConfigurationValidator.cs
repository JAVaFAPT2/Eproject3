using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace VehicleShowroomManagement.Application.Common.Configuration
{
    /// <summary>
    /// Validates application configuration at startup
    /// </summary>
    public static class ConfigurationValidator
    {
        /// <summary>
        /// Validates all required configuration settings
        /// </summary>
        public static async Task<bool> ValidateConfigurationAsync(IConfiguration configuration, ILogger logger)
        {
            var isValid = true;
            var errors = new List<string>();
            var warnings = new List<string>();

            try
            {
                // Check if we're in a deployment environment (Docker/Production)
                var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
                var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
                var isDeployment = isDocker || isProduction;

                logger.LogInformation("Configuration validation - Docker: {IsDocker}, Production: {IsProduction}, Deployment: {IsDeployment}", 
                    isDocker, isProduction, isDeployment);

                // Validate MongoDB connection (CRITICAL - always required)
                var mongoConnectionString = configuration.GetConnectionString("MongoDB");
                if (string.IsNullOrWhiteSpace(mongoConnectionString))
                {
                    errors.Add("MongoDB connection string is missing");
                    isValid = false;
                }
                else
                {
                    // Test MongoDB connection
                    try
                    {
                        var client = new MongoClient(mongoConnectionString);
                        await client.ListDatabaseNamesAsync();
                        logger.LogInformation("MongoDB connection validated successfully");
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"MongoDB connection failed: {ex.Message}");
                        isValid = false;
                    }
                }

                // Validate JWT settings (CRITICAL - always required)
                var jwtKey = configuration["Jwt:Key"];
                if (string.IsNullOrWhiteSpace(jwtKey))
                {
                    errors.Add("JWT Key is missing");
                    isValid = false;
                }
                else if (jwtKey.Length < 32)
                {
                    errors.Add("JWT Key must be at least 32 characters long");
                    isValid = false;
                }

                var jwtIssuer = configuration["Jwt:Issuer"];
                if (string.IsNullOrWhiteSpace(jwtIssuer))
                {
                    errors.Add("JWT Issuer is missing");
                    isValid = false;
                }

                var jwtAudience = configuration["Jwt:Audience"];
                if (string.IsNullOrWhiteSpace(jwtAudience))
                {
                    errors.Add("JWT Audience is missing");
                    isValid = false;
                }

                // Validate Email settings (OPTIONAL - warn but don't fail)
                var smtpHost = configuration["EmailSettings:SmtpHost"];
                if (string.IsNullOrWhiteSpace(smtpHost))
                {
                    warnings.Add("Email SMTP Host is missing - email functionality will be disabled");
                }

                var smtpPort = configuration["EmailSettings:SmtpPort"];
                if (!string.IsNullOrWhiteSpace(smtpHost) && (!int.TryParse(smtpPort, out var port) || port < 1 || port > 65535))
                {
                    warnings.Add("Email SMTP Port must be a valid port number (1-65535)");
                }

                var smtpUsername = configuration["EmailSettings:SmtpUsername"];
                if (string.IsNullOrWhiteSpace(smtpUsername) && !string.IsNullOrWhiteSpace(smtpHost))
                {
                    warnings.Add("Email SMTP Username is missing - email functionality will be disabled");
                }

                var smtpPassword = configuration["EmailSettings:SmtpPassword"];
                if (string.IsNullOrWhiteSpace(smtpPassword) && !string.IsNullOrWhiteSpace(smtpHost))
                {
                    warnings.Add("Email SMTP Password is missing - email functionality will be disabled");
                }

                var fromEmail = configuration["EmailSettings:FromEmail"];
                if (string.IsNullOrWhiteSpace(fromEmail) && !string.IsNullOrWhiteSpace(smtpHost))
                {
                    warnings.Add("Email From Email is missing - email functionality will be disabled");
                }
                else if (!string.IsNullOrWhiteSpace(fromEmail) && !IsValidEmail(fromEmail))
                {
                    warnings.Add("Email From Email is not a valid email address");
                }

                // Validate Cloudinary settings (OPTIONAL - warn but don't fail)
                var cloudName = configuration["CloudinarySettings:CloudName"];
                if (string.IsNullOrWhiteSpace(cloudName))
                {
                    warnings.Add("Cloudinary Cloud Name is missing - image upload functionality will be disabled");
                }

                var apiKey = configuration["CloudinarySettings:ApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(cloudName))
                {
                    warnings.Add("Cloudinary API Key is missing - image upload functionality will be disabled");
                }

                var apiSecret = configuration["CloudinarySettings:ApiSecret"];
                if (string.IsNullOrWhiteSpace(apiSecret) && !string.IsNullOrWhiteSpace(cloudName))
                {
                    warnings.Add("Cloudinary API Secret is missing - image upload functionality will be disabled");
                }

                // Log validation results
                if (isValid)
                {
                    logger.LogInformation("All critical configuration settings validated successfully");
                    
                    if (warnings.Any())
                    {
                        logger.LogWarning("Configuration validation completed with {WarningCount} warnings", warnings.Count);
                        foreach (var warning in warnings)
                        {
                            logger.LogWarning("Configuration Warning: {Warning}", warning);
                        }
                    }
                }
                else
                {
                    logger.LogError("Configuration validation failed with {ErrorCount} critical errors", errors.Count);
                    foreach (var error in errors)
                    {
                        logger.LogError("Configuration Error: {Error}", error);
                    }
                    
                    if (warnings.Any())
                    {
                        logger.LogWarning("Additional configuration warnings:");
                        foreach (var warning in warnings)
                        {
                            logger.LogWarning("Configuration Warning: {Warning}", warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during configuration validation");
                isValid = false;
            }

            return isValid;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}
