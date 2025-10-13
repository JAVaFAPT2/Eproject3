namespace VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile
{
    /// <summary>
    /// Handler for update profile command (unified User schema)
    /// </summary>
    public class UpdateProfileCommandHandler(IRepository<User> userRepository) : IRequestHandler<UpdateProfileCommand>
    {
        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new InvalidOperationException("User not found");
            user.UpdateProfile(request.Name, request.Email, request.Phone, request.Address);
            await userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
