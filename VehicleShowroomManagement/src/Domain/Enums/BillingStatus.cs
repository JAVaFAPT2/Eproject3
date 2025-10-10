namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Billing document payment status
    /// </summary>
    public enum BillingStatus
    {
        /// <summary>
        /// No payment has been made
        /// </summary>
        Unpaid = 1,
        
        /// <summary>
        /// Partial payment has been received
        /// </summary>
        PartiallyPaid = 2,
        
        /// <summary>
        /// Full payment has been received
        /// </summary>
        Paid = 3
    }
}

