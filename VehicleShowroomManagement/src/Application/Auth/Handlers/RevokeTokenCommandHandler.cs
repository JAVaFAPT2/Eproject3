using MediatR;
using VehicleShowroomManagement.Application.Auth.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Auth.Handlers
{
    /// <summary>
    /// Handler for revoking refresh token
    /// </summary>
    public class RevokeTokenCommandHandler(
        IRepository<RefreshToken> refreshTokenRepository)
        : IRequestHandler<RevokeTokenCommand>
    {
        public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await refreshTokenRepository.FirstOrDefaultAsync(rt =>
                rt.Token == request.RefreshToken &&
                !rt.IsDeleted &&
                rt.IsActive);

            if (refreshToken != null)
            {
                refreshToken.Revoke(request.IpAddress, "Revoked by user");
                await refreshTokenRepository.UpdateAsync(refreshToken);
            }
        }
    }
}
