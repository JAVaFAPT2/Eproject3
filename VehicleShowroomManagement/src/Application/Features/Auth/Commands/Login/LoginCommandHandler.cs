using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq.Expressions;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Handler for user login command - accepts username or email
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto?>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IPasswordService passwordService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public async Task<LoginResultDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Find user by username or email
            var users = await _userRepository.FindAsync(u => 
                (u.Username == request.Username || u.Email == request.Username) && 
                u.DeletedAt == null);

            var user = users.FirstOrDefault();

            if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            // Get role name
            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            var roleName = role?.Name ?? "User";

            // Generate JWT token
            var token = GenerateJwtToken(user, roleName);
            var refreshToken = Guid.NewGuid().ToString();
            var expiresAt = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "24"));

            return new LoginResultDto
            {
                UserId = user.Id,
                RoleName = roleName,
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
                    Name = user.Name,
                    Role = roleName,
                    Phone = user.Phone,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        private string GenerateJwtToken(User user, string roleName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("Name", user.Name)
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
