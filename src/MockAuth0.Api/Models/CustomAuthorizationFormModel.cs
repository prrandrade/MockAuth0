using System.Text.Json.Serialization;

namespace MockAuth0.Api.Models
{
    public class CustomAuthorizationFormModel
    {
	    [JsonPropertyName("id")]
		public int Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("responseType")]
        public string ResponseType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("responseMode")]
        public string ResponseMode { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        [JsonPropertyName("auth0Client")]
        public string Auth0Client { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("xClientSku")]
        public string XClientSku { get; set; }

        [JsonPropertyName("xClientVer")]
        public string XClientVer { get; set; }

        [JsonPropertyName("organizationId")]
        public string OrganizationId { get; set; }
    }
}
