using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesightClient.Models
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }
    }
}