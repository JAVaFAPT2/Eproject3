using System.ComponentModel.DataAnnotations;

namespace VehicleShowroomManagement.Application.Email.Models
{
    /// <summary>
    /// Email message model for sending emails
    /// </summary>
    public class EmailMessage
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;

        [EmailAddress]
        public string? Cc { get; set; }

        [EmailAddress]
        public string? Bcc { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public bool IsHtml { get; set; } = true;

        public List<EmailAttachment>? Attachments { get; set; }

        public Dictionary<string, string>? Headers { get; set; }
    }

    /// <summary>
    /// Email attachment model
    /// </summary>
    public class EmailAttachment
    {
        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public byte[] Content { get; set; } = Array.Empty<byte>();

        [Required]
        public string ContentType { get; set; } = "application/octet-stream";

        public string? ContentId { get; set; }
    }
}
