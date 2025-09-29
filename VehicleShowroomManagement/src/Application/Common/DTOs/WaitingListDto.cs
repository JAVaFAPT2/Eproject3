using System;

namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for WaitingList entities
    /// </summary>
    public class WaitingListDto
    {
        public string Id { get; set; } = string.Empty;
        public string WaitId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
