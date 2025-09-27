namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for email service operations
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends password reset email to user
        /// </summary>
        Task SendPasswordResetEmailAsync(string email, string firstName, string resetToken);

        /// <summary>
        /// Sends welcome email to new user
        /// </summary>
        Task SendWelcomeEmailAsync(string email, string firstName, string username, string temporaryPassword);

        /// <summary>
        /// Sends order confirmation email
        /// </summary>
        Task SendOrderConfirmationEmailAsync(string email, string customerName, string orderNumber, decimal totalAmount);

        /// <summary>
        /// Sends invoice email
        /// </summary>
        Task SendInvoiceEmailAsync(string email, string customerName, string invoiceNumber, byte[] invoicePdf);
    }
}