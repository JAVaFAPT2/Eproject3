using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VehicleShowroomManagement.Application.Email.Models;

namespace VehicleShowroomManagement.Application.Email.Services
{
    /// <summary>
    /// SMTP email service implementation using MailKit
    /// </summary>
    public class SmtpEmailService(
        IConfiguration configuration,
        ILogger<SmtpEmailService> logger,
        IEmailTemplateService templateService)
        : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage message)
        {
            try
            {
                var mimeMessage = CreateMimeMessage(message);
                
                using var client = new SmtpClient();
                await ConnectToSmtpAsync(client);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                logger.LogInformation("Email sent successfully to {To}", message.To);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {To}", message.To);
                throw;
            }
        }

        public async Task SendTemplateEmailAsync(string templateName, string to, Dictionary<string, object> variables, string? subject = null)
        {
            try
            {
                var template = await templateService.GetTemplateAsync(templateName);
                var renderedBody = await templateService.RenderTemplateAsync(templateName, variables);

                var message = new EmailMessage
                {
                    To = to,
                    Subject = subject ?? template.Subject,
                    Body = renderedBody,
                    IsHtml = true
                };

                await SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send template email {TemplateName} to {To}", templateName, to);
                throw;
            }
        }

        public async Task SendBulkEmailsAsync(List<EmailMessage> messages)
        {
            var tasks = messages.Select(SendEmailAsync);
            await Task.WhenAll(tasks);
        }

        public bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<EmailTemplate> GetTemplateAsync(string templateName)
        {
            return await templateService.GetTemplateAsync(templateName);
        }

        private MimeMessage CreateMimeMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            
            // From
            var fromEmail = configuration["EmailSettings:FromEmail"] ?? "noreply@showroom.com";
            var fromName = configuration["EmailSettings:FromName"] ?? "Vehicle Showroom";
            mimeMessage.From.Add(new MailboxAddress(fromName, fromEmail));

            // To
            mimeMessage.To.Add(new MailboxAddress("", message.To));

            // CC
            if (!string.IsNullOrEmpty(message.Cc))
            {
                mimeMessage.Cc.Add(new MailboxAddress("", message.Cc));
            }

            // BCC
            if (!string.IsNullOrEmpty(message.Bcc))
            {
                mimeMessage.Bcc.Add(new MailboxAddress("", message.Bcc));
            }

            // Subject
            mimeMessage.Subject = message.Subject;

            // Body
            var bodyBuilder = new BodyBuilder();
            if (message.IsHtml)
            {
                bodyBuilder.HtmlBody = message.Body;
            }
            else
            {
                bodyBuilder.TextBody = message.Body;
            }

            // Attachments
            if (message.Attachments != null && message.Attachments.Any())
            {
                foreach (var attachment in message.Attachments)
                {
                    var mimeAttachment = new MimePart(attachment.ContentType)
                    {
                        Content = new MimeContent(new MemoryStream(attachment.Content)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = attachment.FileName
                    };

                    if (!string.IsNullOrEmpty(attachment.ContentId))
                    {
                        mimeAttachment.ContentId = attachment.ContentId;
                    }

                    bodyBuilder.Attachments.Add(mimeAttachment);
                }
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            return mimeMessage;
        }

        private async Task ConnectToSmtpAsync(SmtpClient client)
        {
            var smtpHost = configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = configuration["EmailSettings:SmtpPassword"];
            var enableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"] ?? "true");

            if (string.IsNullOrEmpty(smtpHost))
            {
                throw new InvalidOperationException("SMTP host is not configured");
            }

            await client.ConnectAsync(smtpHost, smtpPort, enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

            if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
            {
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
            }
        }
    }
}
