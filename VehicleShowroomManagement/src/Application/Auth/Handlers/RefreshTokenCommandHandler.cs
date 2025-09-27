using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VehicleShowroomManagement.Application.Auth.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Auth.Handlers
{
    /// <summary>
    /// Handler for refreshing JWT token using refresh token
    /// </summary>
    public class RefreshTokenCommandHandler(
        IRepository<RefreshToken> refreshTokenRepository,
        IRepository<Employee> employeeRepository,
        IConfiguration configuration)
        : IRequestHandler<RefreshTokenCommand, LoginResultDto>
    {
        public async Task<LoginResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Get refresh token from database
            var refreshToken = await refreshTokenRepository.FirstOrDefaultAsync(rt =>
                rt.Token == request.RefreshToken &&
                !rt.IsDeleted &&
                rt.IsActive);

            if (refreshToken == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            // Get employee
            var employee = await employeeRepository.GetByIdAsync(refreshToken.UserId);
            if (employee == null || employee.IsDeleted || !employee.IsActive)
            {
                throw new UnauthorizedAccessException("Employee not found or inactive");
            }

            // Revoke current refresh token
            refreshToken.Revoke(request.IpAddress, "Replaced by new token");
            await refreshTokenRepository.UpdateAsync(refreshToken);

            // Generate new refresh token
            var newRefreshToken = GenerateRefreshToken(request.IpAddress);
            var newRefreshTokenEntity = RefreshToken.Create(
                newRefreshToken,
                employee.Id,
                DateTime.UtcNow.AddDays(7), // 7 days expiry
                request.IpAddress);

            await refreshTokenRepository.AddAsync(newRefreshTokenEntity);

            // Generate new JWT token
            var jwtToken = GenerateJwtToken(employee);

            return new LoginResultDto
            {
                Token = jwtToken,
                RefreshToken = newRefreshToken,
                TokenExpiresAt = DateTime.UtcNow.AddHours(24),
                RefreshTokenExpiresAt = newRefreshTokenEntity.ExpiresAt,
                RoleName = employee.Role,
                UserId = employee.Id
            };
        }

        private string GenerateJwtToken(Employee employee)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, employee.Id),
                new Claim(JwtRegisteredClaimNames.Email, employee.EmployeeId), // Using EmployeeId as email
                new Claim(JwtRegisteredClaimNames.Name, employee.Name),
                new Claim("username", employee.EmployeeId),
                new Claim("role", employee.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken(string? ipAddress)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
