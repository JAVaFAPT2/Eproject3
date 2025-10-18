using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Common.Configuration;
using VehicleShowroomManagement.Application.Common.Exceptions;
using Polly;
using Microsoft.Extensions.Logging;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of email service with resilience policies
    /// </summary>
    public class EmailService : BaseService, IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly SmtpClient _smtpClient;
        private readonly AsyncPolicy _resiliencePolicy;

        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger, AsyncPolicy resiliencePolicy) 
            : base(logger)
        {
            _settings = options.Value;
            
            _smtpClient = new SmtpClient(_settings.SmtpHost)
            {
                Port = _settings.SmtpPort,
                Credentials = new NetworkCredential(
                    _settings.SmtpUsername, 
                    _settings.SmtpPassword),
                EnableSsl = _settings.EnableSsl
            };
            
            _resiliencePolicy = resiliencePolicy;
        }

        public async Task SendPasswordResetEmailAsync(string email, string firstName, string resetToken)
        {
            LogOperationStart(nameof(SendPasswordResetEmailAsync), new { email, firstName });

            var subject = "Password Reset Request - Vehicle Showroom Management";
            var body = $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>Dear {firstName},</p>
                    <p>You have requested to reset your password. Please use the following token to reset your password:</p>
                    <p><strong>Reset Token: {resetToken}</strong></p>
                    <p>This token will expire in 1 hour.</p>
                    <p>If you did not request this password reset, please ignore this email.</p>
                    <br>
                    <p>Best regards,<br>Vehicle Showroom Management Team</p>
                </body>
                </html>";

            try
            {
                await SendEmailAsync(email, subject, body);
                LogOperationComplete(nameof(SendPasswordResetEmailAsync), new { email });
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(SendPasswordResetEmailAsync), ex, new { email, firstName });
                throw new EmailException($"Failed to send password reset email: {ex.Message}", ex);
            }
        }

        public async Task SendWelcomeEmailAsync(string email, string firstName, string username, string temporaryPassword)
        {
            var subject = "Welcome to Vehicle Showroom Management System";
            var body = $@"
                <html>
                <body>
                    <h2>Welcome to Vehicle Showroom Management</h2>
                    <p>Dear {firstName},</p>
                    <p>Your account has been created successfully. Here are your login credentials:</p>
                    <p><strong>Username:</strong> {username}</p>
                    <p><strong>Temporary Password:</strong> {temporaryPassword}</p>
                    <p>Please change your password after your first login for security reasons.</p>
                    <br>
                    <p>Best regards,<br>Vehicle Showroom Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendOrderConfirmationEmailAsync(string email, string customerName, string orderNumber, decimal totalAmount)
        {
            var subject = $"Order Confirmation - {orderNumber}";
            var body = $@"
                <html>
                <body>
                    <h2>Order Confirmation</h2>
                    <p>Dear {customerName},</p>
                    <p>Thank you for your order. Your order has been confirmed:</p>
                    <p><strong>Order Number:</strong> {orderNumber}</p>
                    <p><strong>Total Amount:</strong> ${totalAmount:F2}</p>
                    <p>We will keep you updated on the status of your order.</p>
                    <br>
                    <p>Best regards,<br>Vehicle Showroom Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendInvoiceEmailAsync(string email, string customerName, string invoiceNumber, byte[] invoicePdf)
        {
            var subject = $"Invoice - {invoiceNumber}";
            var body = $@"
                <html>
                <body>
                    <h2>Invoice</h2>
                    <p>Dear {customerName},</p>
                    <p>Please find attached your invoice: {invoiceNumber}</p>
                    <p>Thank you for your business.</p>
                    <br>
                    <p>Best regards,<br>Vehicle Showroom Management Team</p>
                </body>
                </html>";

            var message = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(email);
            message.Attachments.Add(new Attachment(new MemoryStream(invoicePdf), $"Invoice_{invoiceNumber}.pdf", "application/pdf"));

            await _smtpClient.SendMailAsync(message);
        }

        private async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    var message = new MailMessage
                    {
                        From = new MailAddress(_settings.FromEmail, _settings.FromName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    message.To.Add(email);

                    await _smtpClient.SendMailAsync(message);
                });
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(SendEmailAsync), ex, new { email, subject });
                throw new EmailException($"Failed to send email: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}