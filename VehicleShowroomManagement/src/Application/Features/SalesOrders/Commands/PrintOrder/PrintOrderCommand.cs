using MediatR;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.PrintOrder
{
    /// <summary>
    /// Command for printing sales order
    /// </summary>
    public record PrintOrderCommand(string OrderId) : IRequest<PrintOrderResult>;

    /// <summary>
    /// Result for print order command
    /// </summary>
    public class PrintOrderResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/pdf";
        public string FileName { get; set; } = string.Empty;
    }
}