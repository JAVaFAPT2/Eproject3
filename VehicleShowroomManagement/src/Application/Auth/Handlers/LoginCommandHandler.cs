using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VehicleShowroomManagement.Application.Auth.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Auth.Handlers
{
    /// <summary>
    /// Handler for employee login
    /// </summary>
    public class LoginCommandHandler(
        IRepository<Employee> employeeRepository,
        IRepository<RefreshToken> refreshTokenRepository,
        IConfiguration configuration)
        : IRequestHandler<LoginCommand, LoginResultDto>
    {

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Find employee by employee ID
            var employee = await employeeRepository.FirstOrDefaultAsync(e =>
                e.EmployeeId == request.Email && // Using Email field for employee ID
                !e.IsDeleted &&
                e.IsActive);

            if (employee == null)
            {
                throw new UnauthorizedAccessException("Invalid employee ID or password");
            }

            // For now, we'll skip password verification since Employee entity doesn't have password
            // In a real implementation, you might want to add password field to Employee or use external auth

            // Generate JWT token
            var token = GenerateJwtToken(employee);

            // Generate refresh token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = RefreshToken.Create(
                refreshToken,
                employee.Id,
                DateTime.UtcNow.AddDays(7), // 7 days expiry
                null); // IP address can be added if needed

            await refreshTokenRepository.AddAsync(refreshTokenEntity);

            return new LoginResultDto
            {
                Token = token,
                RefreshToken = refreshToken,
                TokenExpiresAt = DateTime.UtcNow.AddHours(24),
                RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt,
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

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }

}