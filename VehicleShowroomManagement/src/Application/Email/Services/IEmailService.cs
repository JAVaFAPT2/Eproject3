using VehicleShowroomManagement.Application.Email.Models;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Email.Services
{
    /// <summary>
    /// Email service interface for sending emails
    /// </summary>
    public interface IEmailService : IDomainService
    {
        /// <summary>
        /// Send email message
        /// </summary>
        /// <param name="message">Email message to send</param>
        /// <returns>Task representing the async operation</returns>
        Task SendEmailAsync(EmailMessage message);

        /// <summary>
        /// Send email with template
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <param name="to">Recipient email</param>
        /// <param name="variables">Template variables</param>
        /// <param name="subject">Optional custom subject</param>
        /// <returns>Task representing the async operation</returns>
        Task SendTemplateEmailAsync(string templateName, string to, Dictionary<string, object> variables, string? subject = null);

        /// <summary>
        /// Send bulk emails
        /// </summary>
        /// <param name="messages">List of email messages</param>
        /// <returns>Task representing the async operation</returns>
        Task SendBulkEmailsAsync(List<EmailMessage> messages);

        /// <summary>
        /// Validate email address
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool ValidateEmail(string email);

        /// <summary>
        /// Get email template
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <returns>Email template</returns>
        Task<EmailTemplate> GetTemplateAsync(string templateName);
    }
}
