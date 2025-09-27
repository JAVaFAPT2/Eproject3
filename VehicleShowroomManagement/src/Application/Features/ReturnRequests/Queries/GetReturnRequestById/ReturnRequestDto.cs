using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.ReturnRequests.Queries.GetReturnRequestById
{
    public class ReturnRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ProcessedBy { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static ReturnRequestDto FromEntity(ReturnRequest returnRequest)
        {
            return new ReturnRequestDto
            {
                Id = returnRequest.Id,
                OrderId = returnRequest.OrderId,
                CustomerId = returnRequest.CustomerId,
                VehicleId = returnRequest.VehicleId,
                Reason = returnRequest.Reason,
                Description = returnRequest.Description,
                RefundAmount = returnRequest.RefundAmount,
                Status = returnRequest.Status,
                ProcessedBy = returnRequest.ProcessedBy,
                ProcessedAt = returnRequest.ProcessedAt,
                Notes = returnRequest.Notes,
                CreatedAt = returnRequest.CreatedAt,
                UpdatedAt = returnRequest.UpdatedAt
            };
        }
    }
}