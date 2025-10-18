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

            try
            {
                // Validate MongoDB connection
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

                // Validate JWT settings
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

                // Validate Email settings
                var smtpHost = configuration["EmailSettings:SmtpHost"];
                if (string.IsNullOrWhiteSpace(smtpHost))
                {
                    errors.Add("Email SMTP Host is missing");
                    isValid = false;
                }

                var smtpPort = configuration["EmailSettings:SmtpPort"];
                if (!int.TryParse(smtpPort, out var port) || port < 1 || port > 65535)
                {
                    errors.Add("Email SMTP Port must be a valid port number (1-65535)");
                    isValid = false;
                }

                var smtpUsername = configuration["EmailSettings:SmtpUsername"];
                if (string.IsNullOrWhiteSpace(smtpUsername))
                {
                    errors.Add("Email SMTP Username is missing");
                    isValid = false;
                }

                var smtpPassword = configuration["EmailSettings:SmtpPassword"];
                if (string.IsNullOrWhiteSpace(smtpPassword))
                {
                    errors.Add("Email SMTP Password is missing");
                    isValid = false;
                }

                var fromEmail = configuration["EmailSettings:FromEmail"];
                if (string.IsNullOrWhiteSpace(fromEmail))
                {
                    errors.Add("Email From Email is missing");
                    isValid = false;
                }
                else if (!IsValidEmail(fromEmail))
                {
                    errors.Add("Email From Email is not a valid email address");
                    isValid = false;
                }

                // Validate Cloudinary settings
                var cloudName = configuration["CloudinarySettings:CloudName"];
                if (string.IsNullOrWhiteSpace(cloudName))
                {
                    errors.Add("Cloudinary Cloud Name is missing");
                    isValid = false;
                }

                var apiKey = configuration["CloudinarySettings:ApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    errors.Add("Cloudinary API Key is missing");
                    isValid = false;
                }

                var apiSecret = configuration["CloudinarySettings:ApiSecret"];
                if (string.IsNullOrWhiteSpace(apiSecret))
                {
                    errors.Add("Cloudinary API Secret is missing");
                    isValid = false;
                }

                // Log validation results
                if (isValid)
                {
                    logger.LogInformation("All configuration settings validated successfully");
                }
                else
                {
                    logger.LogError("Configuration validation failed with {ErrorCount} errors", errors.Count);
                    foreach (var error in errors)
                    {
                        logger.LogError("Configuration Error: {Error}", error);
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
