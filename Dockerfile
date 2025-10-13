# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj", "VehicleShowroomManagement/src/WebAPI/"]
COPY ["VehicleShowroomManagement/src/Application/VehicleShowroomManagement.Application.csproj", "VehicleShowroomManagement/src/Application/"]
COPY ["VehicleShowroomManagement/src/Domain/VehicleShowroomManagement.Domain.csproj", "VehicleShowroomManagement/src/Domain/"]
COPY ["VehicleShowroomManagement/src/Infrastructure/VehicleShowroomManagement.Infrastructure.csproj", "VehicleShowroomManagement/src/Infrastructure/"]

# Restore dependencies
RUN dotnet restore "VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/VehicleShowroomManagement/src/WebAPI"
RUN dotnet build "VehicleShowroomManagement.WebAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "VehicleShowroomManagement.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VehicleShowroomManagement.WebAPI.dll"]
