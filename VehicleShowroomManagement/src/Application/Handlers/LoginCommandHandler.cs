using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for user login
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Find user by email
            var user = await _userRepository.FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                !u.IsDeleted &&
                u.IsActive);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Verify password (in a real implementation, use proper password hashing)
            var hashedPassword = HashPassword(request.Password);
            if (user.PasswordHash != hashedPassword)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Get user role information
            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            if (role == null)
            {
                throw new InvalidOperationException("User role not found");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user, role);

            return new LoginResultDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId,
                RoleName = role.RoleName,
                Token = token,
                TokenExpiresAt = DateTime.UtcNow.AddHours(24),
                IsActive = user.IsActive
            };
        }

        private string HashPassword(string password)
        {
            // In a real implementation, use BCrypt or similar
            return $"hashed_{password}";
        }

        private string GenerateJwtToken(User user, Role role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim("username", user.Username),
                new Claim("role", role.RoleName),
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
    }

    /// <summary>
    /// Handler for forgot password
    /// </summary>
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;

        public ForgotPasswordCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find user by email
            var user = await _userRepository.FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                !u.IsDeleted);

            if (user == null)
            {
                // Don't reveal if email exists or not for security
                return Unit.Value;
            }

            // Generate password reset token (in real implementation, save to database with expiration)
            var resetToken = Guid.NewGuid().ToString();

            // In a real implementation, you would:
            // 1. Save the reset token to the database with expiration time
            // 2. Send an email with the reset link containing the token

            // For now, just log the token
            Console.WriteLine($"Password reset token for {user.Email}: {resetToken}");

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for reset password
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;

        public ResetPasswordCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // In a real implementation, you would:
            // 1. Find the user by the reset token
            // 2. Check if the token is valid and not expired
            // 3. Update the user's password
            // 4. Clear the reset token

            // For now, just simulate the process
            var hashedPassword = $"hashed_{request.NewPassword}";

            Console.WriteLine($"Password reset with token: {request.Token}");
            Console.WriteLine($"New hashed password: {hashedPassword}");

            return Unit.Value;
        }
    }
}