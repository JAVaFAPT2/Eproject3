using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VehicleShowroomManagement.Application.Auth.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Auth.Handlers
{
    /// <summary>
    /// Handler for user login
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
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

            // Verify password using BCrypt
            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
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
                Token = token,
                TokenExpiresAt = DateTime.UtcNow.AddHours(24),
                RoleName = role.RoleName,
                UserId = user.Id
            };
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
                !u.IsDeleted &&
                u.IsActive);

            if (user == null)
            {
                // Don't reveal if email exists or not for security
                // Always return success to prevent email enumeration attacks
                return Unit.Value;
            }

            // Generate password reset token
            var resetToken = Guid.NewGuid().ToString();

            // Set token expiry (24 hours from now)
            var tokenExpiry = DateTime.UtcNow.AddHours(24);

            // Update user with reset token
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = tokenExpiry;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            // In a real implementation, you would send an email with the reset link
            // For now, log the token (in production, this would be sent via email)
            Console.WriteLine($"Password reset token for {user.Email}: {resetToken}");
            Console.WriteLine($"Token expires at: {tokenExpiry}");

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for reset password
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordService _passwordService;

        public ResetPasswordCommandHandler(
            IRepository<User> userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find user by reset token
            var user = await _userRepository.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == request.Token &&
                !u.IsDeleted &&
                u.IsActive);

            if (user == null)
            {
                throw new ArgumentException("Invalid or expired reset token");
            }

            // Check if token has expired
            if (user.PasswordResetTokenExpiry == null ||
                user.PasswordResetTokenExpiry.Value < DateTime.UtcNow)
            {
                throw new ArgumentException("Reset token has expired");
            }

            // Hash the new password
            var hashedPassword = _passwordService.HashPassword(request.NewPassword);

            // Update user password and clear reset token
            user.PasswordHash = hashedPassword;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            Console.WriteLine($"Password successfully reset for user: {user.Email}");

            return Unit.Value;
        }
    }
}