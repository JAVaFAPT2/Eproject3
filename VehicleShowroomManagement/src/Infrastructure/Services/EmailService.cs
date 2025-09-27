using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of email service
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpHost"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]!),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:SmtpUsername"], 
                    _configuration["EmailSettings:SmtpPassword"]),
                EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]!)
            };
        }

        public async Task SendPasswordResetEmailAsync(string email, string firstName, string resetToken)
        {
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

            await SendEmailAsync(email, subject, body);
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
                From = new MailAddress(_configuration["EmailSettings:FromEmail"]!, _configuration["EmailSettings:FromName"]),
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
            var message = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:FromEmail"]!, _configuration["EmailSettings:FromName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(email);

            await _smtpClient.SendMailAsync(message);
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}