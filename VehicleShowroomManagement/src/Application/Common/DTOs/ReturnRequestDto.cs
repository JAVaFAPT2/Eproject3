using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Return Request entities
    /// </summary>
    public class ReturnRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public string VehicleId { get; set; } = string.Empty;
        public VehicleInfo Vehicle { get; set; } = new VehicleInfo();
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "PENDING";
        public string Description { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
