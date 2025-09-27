namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// User roles in the system
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Human Resources - manages employees and user accounts
        /// </summary>
        HR = 1,
        
        /// <summary>
        /// Vehicle Dealer - manages sales, inventory, and customer relations
        /// </summary>
        Dealer = 2,
        
        /// <summary>
        /// System Administrator - full system access (system role, not user-assignable)
        /// </summary>
        Admin = 3
    }
}