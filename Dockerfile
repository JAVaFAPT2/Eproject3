# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file first
COPY ["VehicleShowroomManagement/VehicleShowroomManagement.sln", "VehicleShowroomManagement/"]

# Copy project files
COPY ["VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj", "VehicleShowroomManagement/src/WebAPI/"]
COPY ["VehicleShowroomManagement/src/Application/VehicleShowroomManagement.Application.csproj", "VehicleShowroomManagement/src/Application/"]
COPY ["VehicleShowroomManagement/src/Domain/VehicleShowroomManagement.Domain.csproj", "VehicleShowroomManagement/src/Domain/"]
COPY ["VehicleShowroomManagement/src/Infrastructure/VehicleShowroomManagement.Infrastructure.csproj", "VehicleShowroomManagement/src/Infrastructure/"]

# Restore dependencies using solution file
RUN dotnet restore "VehicleShowroomManagement/VehicleShowroomManagement.sln"

# Copy all source code
COPY . .

# Build the application from solution
WORKDIR /src
RUN dotnet build "VehicleShowroomManagement/VehicleShowroomManagement.sln" -c Release -o /app/build --no-restore

# Publish the application
FROM build AS publish
WORKDIR /src
RUN dotnet publish "VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VehicleShowroomManagement.WebAPI.dll"]
