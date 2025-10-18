using System.ComponentModel.DataAnnotations;

namespace VehicleShowroomManagement.Application.Common.Configuration
{
    /// <summary>
    /// Email configuration settings with validation
    /// </summary>
    public class EmailSettings
    {
        [Required(ErrorMessage = "SMTP Host is required")]
        public string SmtpHost { get; set; } = string.Empty;

        [Range(1, 65535, ErrorMessage = "SMTP Port must be between 1 and 65535")]
        public int SmtpPort { get; set; } = 587;

        [Required(ErrorMessage = "SMTP Username is required")]
        public string SmtpUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "SMTP Password is required")]
        public string SmtpPassword { get; set; } = string.Empty;

        public bool EnableSsl { get; set; } = true;

        [Required(ErrorMessage = "From Email is required")]
        [EmailAddress(ErrorMessage = "From Email must be a valid email address")]
        public string FromEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "From Name is required")]
        public string FromName { get; set; } = string.Empty;
    }
}
