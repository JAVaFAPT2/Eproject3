using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Handler for user login command - accepts username or email
    /// </summary>
    public class LoginCommandHandler(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IPasswordService passwordService,
        IConfiguration configuration) : IRequestHandler<LoginCommand, LoginResultDto?>
    {
        public async Task<LoginResultDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Find user by username or email
            var users = await userRepository.FindAsync(u => 
                (u.Username == request.Username || u.Email == request.Username) && 
                u.DeletedAt == null, cancellationToken);

            var user = users.FirstOrDefault();

            // Debug: Let's see what users exist in the database
            var allUsers = await userRepository.GetAllAsync(cancellationToken);
            Console.WriteLine($"Total users in database: {allUsers.Count()}");
            foreach (var u in allUsers)
            {
                Console.WriteLine($"User: {u.Username}, DeletedAt: {u.DeletedAt}");
            }

            Console.WriteLine($"Login attempt for: {request.Username}");
            Console.WriteLine($"User found: {user != null}");

            if (user == null || !passwordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            // Get role name
            var role = await roleRepository.GetByIdAsync(user.RoleId, cancellationToken);
            var roleName = role?.Name ?? "User";

            // Generate JWT token
            var token = GenerateJwtToken(user, roleName);
            var refreshToken = Guid.NewGuid().ToString();
            var expiresAt = DateTime.UtcNow.AddHours(Convert.ToDouble(configuration["Jwt:ExpireHours"] ?? "24"));

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
                    Name = user.Name ?? string.Empty,
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("Name", user.Name ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(configuration["Jwt:ExpireHours"] ?? "24")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
