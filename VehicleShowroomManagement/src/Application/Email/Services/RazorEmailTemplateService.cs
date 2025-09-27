using Microsoft.Extensions.Logging;
using RazorEngineCore;
using VehicleShowroomManagement.Application.Email.Models;

namespace VehicleShowroomManagement.Application.Email.Services
{
    /// <summary>
    /// Razor-based email template service implementation
    /// </summary>
    public class RazorEmailTemplateService : IEmailTemplateService
    {
        private readonly ILogger<RazorEmailTemplateService> _logger;
        private readonly Dictionary<string, EmailTemplate> _templates;
        private readonly IRazorEngine _razorEngine;

        public RazorEmailTemplateService(ILogger<RazorEmailTemplateService> logger)
        {
            _logger = logger;
            _templates = new Dictionary<string, EmailTemplate>();
            _razorEngine = new RazorEngineCore.RazorEngine();
            
            InitializeDefaultTemplates();
        }

        public async Task<string> RenderTemplateAsync(string templateName, Dictionary<string, object> variables)
        {
            try
            {
                var template = await GetTemplateAsync(templateName);
                var compiledTemplate = _razorEngine.Compile(template.Body);
                
                var result = await compiledTemplate.RunAsync(variables);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to render template {TemplateName}", templateName);
                throw;
            }
        }

        public async Task<EmailTemplate> GetTemplateAsync(string templateName)
        {
            await Task.CompletedTask; // Async placeholder
            
            if (_templates.TryGetValue(templateName, out var template))
            {
                return template;
            }

            throw new ArgumentException($"Template '{templateName}' not found");
        }

        public async Task SaveTemplateAsync(EmailTemplate template)
        {
            await Task.CompletedTask; // Async placeholder
            _templates[template.TemplateName] = template;
            _logger.LogInformation("Template {TemplateName} saved successfully", template.TemplateName);
        }

        public async Task DeleteTemplateAsync(string templateName)
        {
            await Task.CompletedTask; // Async placeholder
            if (_templates.Remove(templateName))
            {
                _logger.LogInformation("Template {TemplateName} deleted successfully", templateName);
            }
        }

        public async Task<List<EmailTemplate>> GetAllTemplatesAsync()
        {
            await Task.CompletedTask; // Async placeholder
            return _templates.Values.ToList();
        }

        public async Task<bool> TemplateExistsAsync(string templateName)
        {
            await Task.CompletedTask; // Async placeholder
            return _templates.ContainsKey(templateName);
        }

        private void InitializeDefaultTemplates()
        {
            // Password Reset Template
            var passwordResetTemplate = new EmailTemplate
            {
                TemplateName = EmailTemplateTypes.PasswordReset,
                Subject = "Password Reset Request - Vehicle Showroom",
                Body = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Password Reset</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #007bff; color: white; padding: 20px; text-align: center; }
        .content { padding: 20px; background-color: #f9f9f9; }
        .button { display: inline-block; padding: 12px 24px; background-color: #007bff; color: white; text-decoration: none; border-radius: 4px; }
        .footer { padding: 20px; text-align: center; color: #666; font-size: 12px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Password Reset Request</h1>
        </div>
        <div class='content'>
            <p>Hello {{Name}},</p>
            <p>You have requested to reset your password for your Vehicle Showroom account.</p>
            <p>Click the button below to reset your password:</p>
            <p><a href='{{ResetUrl}}' class='button'>Reset Password</a></p>
            <p>This link will expire in {{ExpiryHours}} hours.</p>
            <p>If you did not request this password reset, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>© 2024 Vehicle Showroom Management System</p>
        </div>
    </div>
</body>
</html>"
            };
            _templates[EmailTemplateTypes.PasswordReset] = passwordResetTemplate;

            // Order Confirmation Template
            var orderConfirmationTemplate = new EmailTemplate
            {
                TemplateName = EmailTemplateTypes.OrderConfirmation,
                Subject = "Order Confirmation - {{OrderNumber}}",
                Body = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Order Confirmation</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #28a745; color: white; padding: 20px; text-align: center; }
        .content { padding: 20px; background-color: #f9f9f9; }
        .order-details { background-color: white; padding: 15px; border: 1px solid #ddd; margin: 15px 0; }
        .footer { padding: 20px; text-align: center; color: #666; font-size: 12px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Order Confirmed!</h1>
        </div>
        <div class='content'>
            <p>Dear {{CustomerName}},</p>
            <p>Thank you for your order! Your order has been confirmed and is being processed.</p>
            
            <div class='order-details'>
                <h3>Order Details</h3>
                <p><strong>Order Number:</strong> {{OrderNumber}}</p>
                <p><strong>Vehicle:</strong> {{VehicleName}} ({{VehicleBrand}})</p>
                <p><strong>Total Amount:</strong> ${{TotalAmount}}</p>
                <p><strong>Order Date:</strong> {{OrderDate}}</p>
                <p><strong>Status:</strong> {{Status}}</p>
            </div>
            
            <p>We will contact you soon with delivery details.</p>
            <p>Thank you for choosing our showroom!</p>
        </div>
        <div class='footer'>
            <p>© 2024 Vehicle Showroom Management System</p>
        </div>
    </div>
</body>
</html>"
            };
            _templates[EmailTemplateTypes.OrderConfirmation] = orderConfirmationTemplate;

            // Welcome Email Template
            var welcomeTemplate = new EmailTemplate
            {
                TemplateName = EmailTemplateTypes.WelcomeEmail,
                Subject = "Welcome to Vehicle Showroom Management System",
                Body = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Welcome</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #007bff; color: white; padding: 20px; text-align: center; }
        .content { padding: 20px; background-color: #f9f9f9; }
        .footer { padding: 20px; text-align: center; color: #666; font-size: 12px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to Vehicle Showroom!</h1>
        </div>
        <div class='content'>
            <p>Hello {{Name}},</p>
            <p>Welcome to the Vehicle Showroom Management System!</p>
            <p>Your account has been created successfully with the role: <strong>{{Role}}</strong></p>
            <p>You can now access the system using your credentials:</p>
            <ul>
                <li><strong>Username:</strong> {{Username}}</li>
                <li><strong>Email:</strong> {{Email}}</li>
            </ul>
            <p>Please log in and change your password for security purposes.</p>
        </div>
        <div class='footer'>
            <p>© 2024 Vehicle Showroom Management System</p>
        </div>
    </div>
</body>
</html>"
            };
            _templates[EmailTemplateTypes.WelcomeEmail] = welcomeTemplate;

            // Purchase Order Created Template
            var purchaseOrderTemplate = new EmailTemplate
            {
                TemplateName = EmailTemplateTypes.PurchaseOrderCreated,
                Subject = "New Purchase Order Created - {{OrderNumber}}",
                Body = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Purchase Order Created</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background-color: #ffc107; color: black; padding: 20px; text-align: center; }
        .content { padding: 20px; background-color: #f9f9f9; }
        .order-details { background-color: white; padding: 15px; border: 1px solid #ddd; margin: 15px 0; }
        .footer { padding: 20px; text-align: center; color: #666; font-size: 12px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>New Purchase Order</h1>
        </div>
        <div class='content'>
            <p>A new purchase order has been created and requires your approval.</p>
            
            <div class='order-details'>
                <h3>Purchase Order Details</h3>
                <p><strong>Order Number:</strong> {{OrderNumber}}</p>
                <p><strong>Vehicle:</strong> {{VehicleName}} ({{VehicleBrand}})</p>
                <p><strong>Quantity:</strong> {{Quantity}}</p>
                <p><strong>Unit Price:</strong> ${{Price}}</p>
                <p><strong>Total Amount:</strong> ${{TotalAmount}}</p>
                <p><strong>Order Date:</strong> {{OrderDate}}</p>
                <p><strong>Status:</strong> {{Status}}</p>
            </div>
            
            <p>Please review and approve this purchase order in the system.</p>
        </div>
        <div class='footer'>
            <p>© 2024 Vehicle Showroom Management System</p>
        </div>
    </div>
</body>
</html>"
            };
            _templates[EmailTemplateTypes.PurchaseOrderCreated] = purchaseOrderTemplate;

            _logger.LogInformation("Initialized {Count} default email templates", _templates.Count);
        }
    }
}
