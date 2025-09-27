using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Models.Appointments
{
    /// <summary>
    /// Request model for creating an appointment
    /// </summary>
    public class CreateAppointmentRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string DealerId { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public AppointmentType Type { get; set; } = AppointmentType.Consultation;
        public int DurationMinutes { get; set; } = 60;
        public string? CustomerRequirements { get; set; }
        public string? VehicleInterest { get; set; }
        public string? BudgetRange { get; set; }
    }
}