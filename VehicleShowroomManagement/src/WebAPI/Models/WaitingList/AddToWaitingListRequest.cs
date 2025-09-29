using System;

namespace VehicleShowroomManagement.WebAPI.Models.WaitingList
{
    /// <summary>
    /// Request model for adding to waiting list
    /// </summary>
    public class AddToWaitingListRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public DateTime? PreferredDate { get; set; }
        public string? Notes { get; set; }
    }
}
