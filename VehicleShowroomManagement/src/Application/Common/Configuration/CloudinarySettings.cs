using System.ComponentModel.DataAnnotations;

namespace VehicleShowroomManagement.Application.Common.Configuration
{
    /// <summary>
    /// Cloudinary configuration settings with validation
    /// </summary>
    public class CloudinarySettings
    {
        [Required(ErrorMessage = "Cloudinary Cloud Name is required")]
        public string CloudName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cloudinary API Key is required")]
        public string ApiKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cloudinary API Secret is required")]
        public string ApiSecret { get; set; } = string.Empty;
    }
}
