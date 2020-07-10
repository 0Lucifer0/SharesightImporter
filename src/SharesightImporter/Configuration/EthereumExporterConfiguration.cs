using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class EthereumExporterConfiguration : ExporterConfiguration
    {
        [Required]
        public string EtherscanApiKey { get; set; } = null!;

        [Required] 
        public List<string> Addresses { get; set; } = null!;
    }
}