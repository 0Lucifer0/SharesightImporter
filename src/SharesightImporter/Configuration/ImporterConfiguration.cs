using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class ImporterConfiguration
    {
        [Required]
        public ImporterType ImporterType { get; set; }
    }

    public enum ImporterType
    {
        Sharesight,
        Csv
    }
}