# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Create NuGet.Config to disable fallback folders
RUN echo '<?xml version="1.0" encoding="utf-8"?>' > NuGet.Config && \
    echo '<configuration>' >> NuGet.Config && \
    echo '  <config>' >> NuGet.Config && \
    echo '    <add key="globalPackagesFolder" value="/tmp/nuget-packages" />' >> NuGet.Config && \
    echo '  </config>' >> NuGet.Config && \
    echo '  <fallbackPackageFolders>' >> NuGet.Config && \
    echo '  </fallbackPackageFolders>' >> NuGet.Config && \
    echo '</configuration>' >> NuGet.Config

# Copy all source code
COPY . .

# Restore dependencies using WebAPI project
RUN dotnet restore "VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj" --configfile NuGet.Config

# Build the WebAPI application
WORKDIR /src
RUN dotnet build "VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj" -c Release -o /app/build --no-restore --configfile NuGet.Config

# Publish the application
FROM build AS publish
WORKDIR /src
RUN dotnet publish "VehicleShowroomManagement/src/WebAPI/VehicleShowroomManagement.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VehicleShowroomManagement.WebAPI.dll"]
