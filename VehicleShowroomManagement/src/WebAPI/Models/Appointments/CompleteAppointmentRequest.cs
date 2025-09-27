namespace VehicleShowroomManagement.WebAPI.Models.Appointments
{
    /// <summary>
    /// Request model for completing an appointment
    /// </summary>
    public class CompleteAppointmentRequest
    {
        public string? DealerNotes { get; set; }
        public bool FollowUpRequired { get; set; } = false;
        public DateTime? FollowUpDate { get; set; }
    }
}