using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class CsvImporterConfiguration : ImporterConfiguration
    {
        [Required]
        public string Path { get; set; } = null!;
    }
}