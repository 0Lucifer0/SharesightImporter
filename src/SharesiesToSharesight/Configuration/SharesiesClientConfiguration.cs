using System.ComponentModel.DataAnnotations;

namespace SharesiesToSharesight.Configuration
{
    public class SharesiesClientConfiguration
    {
        [Required] 
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}