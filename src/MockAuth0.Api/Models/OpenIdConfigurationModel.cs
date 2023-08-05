using System.Text.Json.Serialization;

namespace MockAuth0.Api.Models
{
    public class OpenIdConfigurationModel
    {
        [JsonPropertyName("isser")]
        public string Issuer { get; set; }

        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonPropertyName("device_authorization_endpoint")]
        public string DeviceAuthorizationEndpoint { get; set; }

        [JsonPropertyName("userinfo_endpoint")]
        public string UserInfoEndpoint { get; set; }

        [JsonPropertyName("mfa_challenge_endpoint")]
        public string MfaChallengeEndpoint { get; set; }

        [JsonPropertyName("jwks_uri")]
        public string JwksUri { get; set; }

        [JsonPropertyName("registration_endpoint")]
        public string RegistrationEndpoint { get; set; }

        [JsonPropertyName("revocation_endpoint")]
        public string RevocationEndpoint { get; set; }

        [JsonPropertyName("scopes_supported")]
        public List<string> ScopesSupported { get; set; }

        [JsonPropertyName("response_types_supported")]
        public List<string> ResponseTypesSupported { get; set; }

        [JsonPropertyName("code_challenge_methods_supported")]
        public List<string> CodeChallengeMethodsSupported { get; set; }

        [JsonPropertyName("response_modes_supported")]
        public List<string> ResponseModesSupported { get; set; }

        [JsonPropertyName("subject_types_supported")]
        public List<string> SubjectTypesSupported { get; set; }

        [JsonPropertyName("id_token_signing_alg_values_supported")]
        public List<string> IdTokenSigningAlgValuesSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        public List<string> TokenEndpointAuthMethodsSupported { get; set; }

        [JsonPropertyName("claims_supported")]
        public List<string> ClaimsSupported { get; set; }

        [JsonPropertyName("request_uri_parameter_supported")]
        public bool RequestUriParameterSupported { get; set; }

        [JsonPropertyName("request_parameter_supported")]
        public bool RequestParameterSupported { get; set; }

        [JsonPropertyName("token_endpoint_auth_signing_alg_values_supported")]
        public List<string> TokenEndpointAuthSigningAlgValuesSupported { get; set; }
    }
}
