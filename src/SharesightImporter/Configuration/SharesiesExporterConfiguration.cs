using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class SharesiesExporterConfiguration : ExporterConfiguration
    {
        [Required] 
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}