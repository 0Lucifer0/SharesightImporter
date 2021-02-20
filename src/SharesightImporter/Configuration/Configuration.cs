using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class Configuration
    {
        [Required]
        public List<ImporterConfiguration>  Importers { get; set; } = null!;

        [Required]
        public List<ExporterConfiguration> Exporters { get; set; } = null!;
    }
}
