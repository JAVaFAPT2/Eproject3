namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Enumeration for appointment status
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// Appointment is scheduled but not confirmed
        /// </summary>
        Scheduled = 0,

        /// <summary>
        /// Appointment is confirmed by dealer
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// Appointment is currently in progress
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Appointment has been completed
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Appointment has been cancelled
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// Customer did not show up for appointment
        /// </summary>
        NoShow = 5,

        /// <summary>
        /// Appointment has been rescheduled
        /// </summary>
        Rescheduled = 6
    }
}