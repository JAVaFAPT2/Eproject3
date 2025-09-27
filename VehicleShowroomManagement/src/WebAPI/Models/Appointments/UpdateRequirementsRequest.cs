namespace VehicleShowroomManagement.WebAPI.Models.Appointments
{
    /// <summary>
    /// Request model for updating appointment requirements
    /// </summary>
    public class UpdateRequirementsRequest
    {
        public string CustomerRequirements { get; set; } = string.Empty;
        public string? VehicleInterest { get; set; }
        public string? BudgetRange { get; set; }
    }
}