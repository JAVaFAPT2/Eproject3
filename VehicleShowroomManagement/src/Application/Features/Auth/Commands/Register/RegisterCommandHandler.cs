using VehicleShowroomManagement.Domain.Services;
namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Register
{
    /// <summary>
    /// Handler for public user registration
    /// </summary>
    public class RegisterCommandHandler(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IPasswordService passwordService) : IRequestHandler<RegisterCommand, string>
    {
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if username already exists
            var existingUsers = await userRepository.FindAsync(u => u.Username == request.Username && u.DeletedAt == null, cancellationToken);
            if (existingUsers.Any())
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check if email already exists
            existingUsers = await userRepository.FindAsync(u => u.Email == request.Email && u.DeletedAt == null, cancellationToken);
            if (existingUsers.Any())
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Find Customer role by name
            var customerRoles = await roleRepository.FindAsync(r => r.Name == "Customer", cancellationToken);
            var customerRole = customerRoles.FirstOrDefault();
            
            if (customerRole == null)
            {
                throw new InvalidOperationException("Customer role not found in the system");
            }

            // Hash password
            var passwordHash = passwordService.HashPassword(request.Password);

            // Create user with Customer role
            var user = new User(
                request.Username,
                passwordHash,
                request.Username, // Use username as name initially
                request.Email,
                customerRole.Id,
                phone: null,
                address: null,
                hireDate: null);

            // Add to repository
            await userRepository.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }
}

