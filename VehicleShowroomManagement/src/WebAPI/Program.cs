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

var builder = WebApplication.CreateBuilder(args);

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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

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
