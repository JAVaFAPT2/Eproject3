namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Service for password operations
    /// </summary>
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}