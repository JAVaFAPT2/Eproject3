using VehicleShowroomManagement.Application.Email.Models;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Email.Services
{
    /// <summary>
    /// Email template service interface for managing email templates
    /// </summary>
    public interface IEmailTemplateService : IDomainService
    {
        /// <summary>
        /// Render template with variables
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <param name="variables">Template variables</param>
        /// <returns>Rendered template content</returns>
        Task<string> RenderTemplateAsync(string templateName, Dictionary<string, object> variables);

        /// <summary>
        /// Get template by name
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <returns>Email template</returns>
        Task<EmailTemplate> GetTemplateAsync(string templateName);

        /// <summary>
        /// Save template
        /// </summary>
        /// <param name="template">Template to save</param>
        /// <returns>Task representing the async operation</returns>
        Task SaveTemplateAsync(EmailTemplate template);

        /// <summary>
        /// Delete template
        /// </summary>
        /// <param name="templateName">Template name to delete</param>
        /// <returns>Task representing the async operation</returns>
        Task DeleteTemplateAsync(string templateName);

        /// <summary>
        /// Get all templates
        /// </summary>
        /// <returns>List of all templates</returns>
        Task<List<EmailTemplate>> GetAllTemplatesAsync();

        /// <summary>
        /// Check if template exists
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> TemplateExistsAsync(string templateName);
    }
}
