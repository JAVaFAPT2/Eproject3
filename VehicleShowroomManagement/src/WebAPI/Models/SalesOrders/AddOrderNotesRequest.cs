namespace VehicleShowroomManagement.WebAPI.Models.SalesOrders
{
    /// <summary>
    /// Request model for adding notes to order
    /// </summary>
    public class AddOrderNotesRequest
    {
        public string Notes { get; set; } = string.Empty;
        public string NoteType { get; set; } = "General"; // General, Internal, Customer, Delivery
        public string AddedBy { get; set; } = string.Empty;
        public bool IsInternal { get; set; } = false;
        public DateTime? ScheduledDate { get; set; }
    }
}