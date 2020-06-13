using System.ComponentModel.DataAnnotations;

namespace SharesiesToSharesight.Configuration
{
    public class Configuration
    {
        [Required]
        public SharesiesClientConfiguration SharesiesClient { get; set; } = null!;

        [Required]
        public SharesightClientConfiguration SharesightClient { get; set; } = null!;
    }
}
