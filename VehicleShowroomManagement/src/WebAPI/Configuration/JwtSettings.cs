using System.ComponentModel.DataAnnotations;

namespace VehicleShowroomManagement.Application.Common.Configuration
{
    /// <summary>
    /// JWT configuration settings with validation
    /// </summary>
    public class JwtSettings
    {
        [Required(ErrorMessage = "JWT Key is required")]
        [MinLength(32, ErrorMessage = "JWT Key must be at least 32 characters long for security")]
        public string Key { get; set; } = string.Empty;

        [Required(ErrorMessage = "JWT Issuer is required")]
        public string Issuer { get; set; } = string.Empty;

        [Required(ErrorMessage = "JWT Audience is required")]
        public string Audience { get; set; } = string.Empty;

        [Range(1, 168, ErrorMessage = "JWT ExpireHours must be between 1 and 168 hours")]
        public int ExpireHours { get; set; } = 24;
    }
}
