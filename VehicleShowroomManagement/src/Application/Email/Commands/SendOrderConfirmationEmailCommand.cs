using MediatR;

namespace VehicleShowroomManagement.Application.Email.Commands
{
    /// <summary>
    /// Command to send order confirmation email
    /// </summary>
    public class SendOrderConfirmationEmailCommand : IRequest
    {
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
