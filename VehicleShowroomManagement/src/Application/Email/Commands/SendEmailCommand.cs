using MediatR;

namespace VehicleShowroomManagement.Application.Email.Commands
{
    /// <summary>
    /// Command to send email
    /// </summary>
    public class SendEmailCommand : IRequest
    {
        public string To { get; set; } = string.Empty;
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public List<EmailAttachmentCommand>? Attachments { get; set; }
    }

    /// <summary>
    /// Email attachment command
    /// </summary>
    public class EmailAttachmentCommand
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
        public string? ContentId { get; set; }
    }
}
