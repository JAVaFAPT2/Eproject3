namespace VehicleShowroomManagement.WebAPI.Models.Billing
{
    /// <summary>
    /// Request model for applying credit
    /// </summary>
    public class ApplyCreditRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string AppliedBy { get; set; } = string.Empty;
    }
}