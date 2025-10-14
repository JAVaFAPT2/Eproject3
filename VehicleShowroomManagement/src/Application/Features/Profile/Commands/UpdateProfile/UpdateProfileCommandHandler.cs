namespace VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile
{
    /// <summary>
    /// Handler for update profile command (unified User schema)
    /// </summary>
    public class UpdateProfileCommandHandler(IRepository<User> userRepository) : IRequestHandler<UpdateProfileCommand>
    {
        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            // Debug: Log the incoming request
            Console.WriteLine($"UpdateProfileCommand - UserId: {request.UserId}, Name: {request.Name}, Email: {request.Email}, Phone: {request.Phone}");
            
            var users = await userRepository.FindAsync(u => u.Id == request.UserId && u.DeletedAt == null, cancellationToken);
            var user = users.FirstOrDefault() ?? throw new InvalidOperationException("User not found");
            
            // Debug: Log before update
            Console.WriteLine($"Before update - Name: {user.Name}, Email: {user.Email}, Phone: {user.Phone}");
            
            user.UpdateProfile(request.Name, request.Email, request.Phone, request.Address);
            
            // Debug: Log after update
            Console.WriteLine($"After update - Name: {user.Name}, Email: {user.Email}, Phone: {user.Phone}");
            
            await userRepository.UpdateAsync(user, cancellationToken);
            
            Console.WriteLine("UpdateAsync completed");
        }
    }
}
