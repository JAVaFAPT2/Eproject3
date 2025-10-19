using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VehicleShowroomManagement.Application.DependencyInjection;
using VehicleShowroomManagement.Infrastructure.DependencyInjection;
using VehicleShowroomManagement.Infrastructure.Persistence;
using VehicleShowroomManagement.WebAPI.DependencyInjection;
using VehicleShowroomManagement.Application.Common.Configuration;
using DotNetEnv;
using Microsoft.AspNetCore.DataProtection;
using Serilog;
using Serilog.Events;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/vehicleshowroom-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Vehicle Showroom Management API");

    // Load .env file if it exists (look in project root)
    var projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
    var envFile = Path.Combine(projectRoot, ".env");
    if (File.Exists(envFile))
    {
        Env.Load(envFile);
        Log.Information("Loaded .env file from {EnvFile}", envFile);
    }

    var builder = WebApplication.CreateBuilder(args);
    
    // Configure environment-specific settings
    if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
    {
        builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true);
    }
    
    // Use Serilog for logging
    builder.Host.UseSerilog();

// Configure URLs based on environment
var environmentName = builder.Environment.EnvironmentName;
var isProduction = builder.Environment.IsProduction();
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
Log.Information("Environment: {Environment}, IsProduction: {IsProduction}, IsDocker: {IsDocker}", 
    environmentName, isProduction, isDocker);

if (isProduction || isDocker)
{
    // In production or Docker, use HTTP only (no SSL certificates needed)
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    var urls = $"http://0.0.0.0:{port}";
    Log.Information("Production/Docker URLs: {Urls}", urls);
    builder.WebHost.UseUrls(urls);
}
else
{
    // In local development (non-Docker), use localhost with both HTTP and HTTPS
    Log.Information("Local Development URLs: http://localhost:8090, https://localhost:8091");
    builder.WebHost.UseUrls("http://localhost:8090", "https://localhost:8091");
}

// Configure Autofac as the service provider factory
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configure Autofac container
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register Autofac modules
    containerBuilder.RegisterModule<ApplicationModule>();
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
    containerBuilder.RegisterModule<WebApiModule>();
});

// Configure Data Protection for containerized environments
if (builder.Environment.IsProduction() || isDocker)
{
    // Create keys directory if it doesn't exist
    var keysDirectory = Path.Combine("/app", "keys");
    if (!Directory.Exists(keysDirectory))
    {
        Directory.CreateDirectory(keysDirectory);
    }
    
    builder.Services.AddDataProtection()
        .SetApplicationName("VehicleShowroomManagement")
        .SetDefaultKeyLifetime(TimeSpan.FromDays(90))
        .PersistKeysToFileSystem(new DirectoryInfo(keysDirectory));
}

// Configure IOptions with validation
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Add services to the container
builder.Services.AddControllers();

// Add health checks
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Vehicle Showroom Management API",
        Version = "v1",
        Description = "API for Vehicle Showroom Management System with DDD, Clean Architecture, CQRS, and Autofac"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Authorization policies for role-based access
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HR", policy => policy.RequireRole("HR"));
    options.AddPolicy("Dealer", policy => policy.RequireRole("Dealer"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFE", policy =>
    {
        var origins = builder.Configuration["Cors:Origins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? ["http://localhost:3000"];
        
        Log.Information("CORS Origins configured: {Origins}", string.Join(", ", origins));
        
        if (origins.Contains("*"))
        {
            // For wildcard origins, don't allow credentials
            Log.Information("Using wildcard CORS policy (no credentials)");
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            // For specific origins, allow credentials
            Log.Information("Using specific origins CORS policy (with credentials)");
            policy.WithOrigins(origins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

// Only use HTTPS redirection in development (when HTTPS is available)
if (!isProduction && !isDocker)
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowFE");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add health check endpoint
app.MapHealthChecks("/health");

// Database Initialization and Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VehicleShowroomDbContext>();

        // Initialize MongoDB collections with indexes
        await context.InitializeCollectionsAsync();

        // Initialize MongoDB performance indexes
        var indexInitializer = services.GetRequiredService<MongoDbIndexInitializer>();
        await indexInitializer.InitializeIndexesAsync();

        // Seed initial data
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing or seeding the database.");
    }
}

// Validate configuration before starting the application
var configLogger = app.Services.GetRequiredService<ILogger<Program>>();
var configuration = app.Services.GetRequiredService<IConfiguration>();

var isConfigValid = await ConfigurationValidator.ValidateConfigurationAsync(configuration, configLogger);
if (!isConfigValid)
{
    Log.Fatal("Configuration validation failed. Application cannot start.");
    Environment.Exit(1);
}

Log.Information("Configuration validation completed successfully");

// Log startup information
Log.Information("Application configuration:");
Log.Information("- Environment: {Environment}", builder.Environment.EnvironmentName);
Log.Information("- IsProduction: {IsProduction}", isProduction);
Log.Information("- IsDocker: {IsDocker}", isDocker);
Log.Information("- Data Protection Keys Directory: {KeysDirectory}", isDocker ? "/app/keys" : "Default");
Log.Information("- HTTPS Redirection: {HttpsRedirect}", !isProduction && !isDocker ? "Enabled" : "Disabled");

app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
