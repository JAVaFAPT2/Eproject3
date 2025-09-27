namespace VehicleShowroomManagement.WebAPI.Models.Appointments
{
    /// <summary>
    /// Request model for cancelling an appointment
    /// </summary>
    public class CancelAppointmentRequest
    {
        public string? Reason { get; set; }
    }
}