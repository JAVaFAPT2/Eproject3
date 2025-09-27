using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Handler for user login command - production ready
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public async Task<LoginResultDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Find user by username or email
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                user = await _userRepository.GetByEmailAsync(request.Username);
            }

            if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);
            var refreshToken = Guid.NewGuid().ToString();
            var expiresAt = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "24"));

            return new LoginResultDto
            {
                UserId = user.Id,
                RoleName = user.Role.ToString(),
                Token = token,
                RefreshToken = refreshToken,
                TokenExpiresAt = expiresAt,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(30),
                AccessToken = token,
                ExpiresAt = expiresAt,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Phone = user.Phone,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        private string GenerateJwtToken(Domain.Entities.User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "24")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}