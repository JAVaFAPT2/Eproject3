namespace VehicleShowroomManagement.WebAPI.Models.Appointments
{
    /// <summary>
    /// Request model for rescheduling an appointment
    /// </summary>
    public class RescheduleAppointmentRequest
    {
        public DateTime NewAppointmentDate { get; set; }
    }
}