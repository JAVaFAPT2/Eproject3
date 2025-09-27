using MediatR;

namespace VehicleShowroomManagement.Application.Email.Commands
{
    /// <summary>
    /// Command to send email using template
    /// </summary>
    public class SendTemplateEmailCommand : IRequest
    {
        public string TemplateName { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string? Subject { get; set; }
        public Dictionary<string, object> Variables { get; set; } = new();
    }
}
