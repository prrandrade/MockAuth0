using Flurl;

namespace MockAuth0.Api.Models
{
    public class AddressesModel
    {
        public string Issuer { get; set; }

        public string Authorization { get; set; }
        public string AuthorizationEndpoint => Url.Combine(Issuer, Authorization);

        public string Token { get; set; }
        public string TokenEndpoint => Url.Combine(Issuer, Token);

        public string DeviceAuthorization { get; set; }
        public string DeviceAuthorizationEndpoint => Url.Combine(Issuer, DeviceAuthorization);

        public string UserInfo { get; set; }
        public string UserInfoEndpoint => Url.Combine(Issuer, UserInfo);

        public string MfaChallenge { get; set; }
        public string MfaChallengeEndpoint => Url.Combine(Issuer, MfaChallenge);

        public string JwksFile { get; set; }
        public string JwksFileEndpoint => Url.Combine(Issuer, JwksFile);

        public string Registration { get; set; }
        public string RegistrationEndpoint => Url.Combine(Issuer, Registration);

        public string Revokation { get; set; }
        public string RevokationEndpoint => Url.Combine(Issuer, Revokation);
    }
}
