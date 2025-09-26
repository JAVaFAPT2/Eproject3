namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Service for password hashing and verification
    /// </summary>
    public interface IPasswordService : IDomainService
    {
        /// <summary>
        /// Hashes a plain text password
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a plain text password against a hashed password
        /// </summary>
        bool VerifyPassword(string password, string hashedPassword);
    }
}
