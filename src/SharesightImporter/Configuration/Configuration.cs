using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class Configuration
    {
        [Required]
        public SharesiesClientConfiguration SharesiesClient { get; set; } = null!;

        [Required]
        public SharesightClientConfiguration SharesightClient { get; set; } = null!;
    }
}
