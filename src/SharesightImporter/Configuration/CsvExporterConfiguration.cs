using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class CsvExporterConfiguration : ExporterConfiguration
    {
        [Required]
        public string Path { get; set; } = null!;
    }
}