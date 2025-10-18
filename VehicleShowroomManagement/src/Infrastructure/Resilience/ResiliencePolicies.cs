using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using System;

namespace VehicleShowroomManagement.Infrastructure.Resilience
{
    /// <summary>
    /// Resilience policies for external service calls
    /// </summary>
    public static class ResiliencePolicies
    {
        /// <summary>
        /// Retry policy for Cloudinary operations
        /// </summary>
        public static AsyncRetryPolicy GetCloudinaryRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Cloudinary retry {retryCount} in {timespan} seconds due to: {outcome.Exception?.Message}");
                    });
        }

        /// <summary>
        /// Circuit breaker policy for Cloudinary operations
        /// </summary>
        public static AsyncCircuitBreakerPolicy GetCloudinaryCircuitBreakerPolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMinutes(1),
                    onBreak: (exception, duration) =>
                    {
                        Console.WriteLine($"Cloudinary circuit breaker opened for {duration} due to: {exception.Message}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Cloudinary circuit breaker reset");
                    });
        }

        /// <summary>
        /// Combined policy for Cloudinary operations (retry + circuit breaker)
        /// </summary>
        public static AsyncPolicy GetCloudinaryPolicy()
        {
            return GetCloudinaryRetryPolicy()
                .WrapAsync(GetCloudinaryCircuitBreakerPolicy());
        }

        /// <summary>
        /// Retry policy for Email operations
        /// </summary>
        public static AsyncRetryPolicy GetEmailRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Email retry {retryCount} in {timespan} seconds due to: {outcome.Exception?.Message}");
                    });
        }

        /// <summary>
        /// Circuit breaker policy for Email operations
        /// </summary>
        public static AsyncCircuitBreakerPolicy GetEmailCircuitBreakerPolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMinutes(2),
                    onBreak: (exception, duration) =>
                    {
                        Console.WriteLine($"Email circuit breaker opened for {duration} due to: {exception.Message}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Email circuit breaker reset");
                    });
        }

        /// <summary>
        /// Combined policy for Email operations (retry + circuit breaker)
        /// </summary>
        public static AsyncPolicy GetEmailPolicy()
        {
            return GetEmailRetryPolicy()
                .WrapAsync(GetEmailCircuitBreakerPolicy());
        }
    }
}
