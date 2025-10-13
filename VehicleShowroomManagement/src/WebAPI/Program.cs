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
using DotNetEnv;
using Microsoft.AspNetCore.DataProtection;

// Load .env file if it exists (look in project root)
var projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
var envFile = Path.Combine(projectRoot, ".env");
if (File.Exists(envFile))
{
    Env.Load(envFile);
}

var builder = WebApplication.CreateBuilder(args);

// Configure URLs based on environment
var environmentName = builder.Environment.EnvironmentName;
var isProduction = builder.Environment.IsProduction();
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
Console.WriteLine($"Environment: {environmentName}, IsProduction: {isProduction}, IsDocker: {isDocker}");

if (isProduction || isDocker)
{
    // In production or Docker, use HTTP only (no SSL certificates needed)
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    var urls = $"http://0.0.0.0:{port}";
    Console.WriteLine($"Production/Docker URLs: {urls}");
    builder.WebHost.UseUrls(urls);
}
else
{
    // In local development (non-Docker), use localhost with both HTTP and HTTPS
    Console.WriteLine("Local Development URLs: http://localhost:8090, https://localhost:8091");
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
    builder.Services.AddDataProtection()
        .SetApplicationName("VehicleShowroomManagement")
        .SetDefaultKeyLifetime(TimeSpan.FromDays(90));
}

// Add services to the container
builder.Services.AddControllers();

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
        policy.WithOrigins(builder.Configuration["Cors:Origins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? ["http://localhost:3000"])
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowFE");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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

app.Run();
