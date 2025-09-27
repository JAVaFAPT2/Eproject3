using MediatR;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Commands
{
    /// <summary>
    /// Command to transfer vehicle ownership
    /// </summary>
    public class TransferOwnershipCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string NewOwnerName { get; set; } = string.Empty;
        public string NewOwnerAddress { get; set; } = string.Empty;
        public string? NewOwnerPhone { get; set; }
        public string? NewOwnerEmail { get; set; }
    }
}
