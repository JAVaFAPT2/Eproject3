namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Enumeration for appointment types
    /// </summary>
    public enum AppointmentType
    {
        /// <summary>
        /// General consultation about vehicles
        /// </summary>
        Consultation = 0,

        /// <summary>
        /// Test drive appointment
        /// </summary>
        TestDrive = 1,

        /// <summary>
        /// Vehicle inspection appointment
        /// </summary>
        Inspection = 2,

        /// <summary>
        /// Purchase discussion and negotiation
        /// </summary>
        Purchase = 3,

        /// <summary>
        /// Vehicle delivery appointment
        /// </summary>
        Delivery = 4,

        /// <summary>
        /// Service and maintenance appointment
        /// </summary>
        Service = 5,

        /// <summary>
        /// Follow-up meeting
        /// </summary>
        FollowUp = 6,

        /// <summary>
        /// Online virtual consultation
        /// </summary>
        OnlineConsultation = 7
    }
}