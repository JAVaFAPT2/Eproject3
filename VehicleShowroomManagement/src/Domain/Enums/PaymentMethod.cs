namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Payment methods accepted by the system
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Cash payment
        /// </summary>
        Cash = 1,
        
        /// <summary>
        /// Credit card payment
        /// </summary>
        CreditCard = 2,
        
        /// <summary>
        /// Bank transfer
        /// </summary>
        BankTransfer = 3,
        
        /// <summary>
        /// Financing/loan
        /// </summary>
        Financing = 4,
        
        /// <summary>
        /// Trade-in vehicle
        /// </summary>
        TradeIn = 5
    }
}