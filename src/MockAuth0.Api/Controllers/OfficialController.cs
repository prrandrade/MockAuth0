using Microsoft.AspNetCore.Mvc;
using MockAuth0.Api.Models;
using MockAuth0.Api.Services;
using System.Text.Json;

namespace MockAuth0.Api.Controllers
{
    [ApiController]
    public class OfficialController : ControllerBase
    {
        private readonly IJwtGeneratorService _jwtGeneratorService;
        private readonly List<OrganizationConfigurationModel> _organizations;
        private readonly AddressesModel _addressesModel;
        private readonly IDatabaseService _databaseService;

        public OfficialController(List<OrganizationConfigurationModel> organizations, IJwtGeneratorService jwtGeneratorService, AddressesModel addressesModel, IDatabaseService databaseService)
        {
            _organizations = organizations;
            _addressesModel = addressesModel;
            _jwtGeneratorService = jwtGeneratorService;
            _databaseService = databaseService;
        }


        [HttpGet]
        [Route(".well-known/openid-configuration")]
        public IActionResult GetOpenIdConfiguration()
        {
            var issuer =  _addressesModel.Issuer;
            var authorizationEndpoint = _addressesModel.AuthorizationEndpoint;
            var tokenEndpoint = _addressesModel.TokenEndpoint;
            var deviceAuthorizationEndpoint = _addressesModel.DeviceAuthorizationEndpoint;
            var userInfo = _addressesModel.UserInfo;
            var mfaChallengeEndpoint = _addressesModel.MfaChallengeEndpoint;
            var jwksFile = _addressesModel.JwksFileEndpoint;
            var registrationEndpoint = _addressesModel.RegistrationEndpoint;
            var revokationEndpoint = _addressesModel.RevokationEndpoint; 

            var str = $"{{\"issuer\":\"{issuer}\",\"authorization_endpoint\":\"{authorizationEndpoint}\",\"token_endpoint\":\"{tokenEndpoint}\",\"device_authorization_endpoint\":\"{deviceAuthorizationEndpoint}\",\"userinfo_endpoint\":\"{userInfo}\",\"mfa_challenge_endpoint\":\"{mfaChallengeEndpoint}\",\"jwks_uri\":\"{jwksFile}\",\"registration_endpoint\":\"{registrationEndpoint}\",\"revocation_endpoint\":\"{revokationEndpoint}\",\"scopes_supported\":[\"openid\",\"profile\",\"offline_access\",\"name\",\"given_name\",\"family_name\",\"nickname\",\"email\",\"email_verified\",\"picture\",\"created_at\",\"identities\",\"phone\",\"address\"],\"response_types_supported\":[\"code\",\"token\",\"id_token\",\"code token\",\"code id_token\",\"token id_token\",\"code token id_token\"],\"code_challenge_methods_supported\":[\"S256\",\"plain\"],\"response_modes_supported\":[\"query\",\"fragment\",\"form_post\"],\"subject_types_supported\":[\"public\"],\"id_token_signing_alg_values_supported\":[\"HS256\",\"RS256\"],\"token_endpoint_auth_methods_supported\":[\"client_secret_basic\",\"client_secret_post\",\"private_key_jwt\"],\"claims_supported\":[\"aud\",\"auth_time\",\"created_at\",\"email\",\"email_verified\",\"exp\",\"family_name\",\"given_name\",\"iat\",\"identities\",\"iss\",\"name\",\"nickname\",\"phone_number\",\"picture\",\"sub\"],\"request_uri_parameter_supported\":false,\"request_parameter_supported\":false,\"token_endpoint_auth_signing_alg_values_supported\":[\"RS256\",\"RS384\",\"PS256\"]}}";
            return Ok(JsonSerializer.Deserialize<OpenIdConfigurationModel>(str));
        }

        [HttpGet]
        [Route(".well-known/jwks.json")]
        public IActionResult GetJwks()
        {
            return Content(_jwtGeneratorService.JwksStr, "application/json");
        }

        [HttpGet]
        [Route("authorize")]
        public IActionResult AuthorizationEndpont([FromQuery(Name = "client_id")] string clientId, [FromQuery(Name = "redirect_uri")] string redirectUri, [FromQuery(Name = "response_type")] string responseType, [FromQuery(Name = "scope")] string scope, [FromQuery(Name = "response_mode")] string responseMode, [FromQuery] string nonce, [FromQuery] string auth0Client, [FromQuery(Name="organization")] string organizationId, [FromQuery(Name = "login_hint")] string loginHint, [FromQuery] string state, [FromQuery(Name = "x-client-SKU")] string xClientSku, [FromQuery(Name = "x-client-ver")] string xClientVer)
        {
            var currentOrganization = _organizations.FirstOrDefault(x => x.Id == organizationId);
            if (currentOrganization == null)
            {
                return BadRequest("Organization not found");
            }

            var file = System.IO.File.ReadAllText("forms/form.html");
            file = file
                .Replace("{{clientId}}", clientId)
                .Replace("{{redirectUri}}", redirectUri)
                .Replace("{{responseType}}", responseType)
                .Replace("{{scope}}", scope)
                .Replace("{{responseMode}}", responseMode)
                .Replace("{{nonce}}", nonce)
                .Replace("{{auth0Client}}", auth0Client)
                .Replace("{{loginHint}}", loginHint)
                .Replace("{{state}}", state)
                .Replace("{{xClientSku}}", xClientSku)
                .Replace("{{xClientVer}}", xClientVer);

            file = file
                .Replace("{{organizationId}}", currentOrganization.Id)
                .Replace("{{organizationName}}", currentOrganization.Name)
                .Replace("{{organizationRedirectUriForLogin}}", currentOrganization.RedirectUriForLogin)
                .Replace("{{organizationRedirectUriForLogout}}", currentOrganization.RedirectUriForLogout);

            var claimFinalPart = "";
            var claimPart = System.IO.File.ReadAllText("forms/formPart.html");
            foreach (var possibleClaim in currentOrganization.Claims)
            {
                var currentInfo = _databaseService.GetByEmailAndClientIdAndOrganizationId(loginHint, clientId, organizationId);
                
                var claimTempPart = claimPart;
                claimTempPart = claimTempPart
                    .Replace("{{claimFullName}}", possibleClaim.FullName)
                    .Replace("{{claimShortName}}", possibleClaim.ShortName)
                    .Replace("{{claimValue}}",  (currentInfo?.ClaimsAndValues.FirstOrDefault(x => x.Key == possibleClaim.FullName))?.Value ?? "");                

                claimFinalPart += claimTempPart;
            }

            file = file.Replace("{{claims}}", claimFinalPart);


            return Content(file, "text/html");
        }

        [HttpGet]
        [Route("oauth/token")]
        public IActionResult TokenEndpoint()
        {
            return Ok();
        }

        [HttpGet]
        [Route("oauth/device/code")]
        public IActionResult DeviceAuthorizationEndpoint()
        {
            return Ok();
        }

        [HttpGet]
        [Route("userinfo")]
        public IActionResult UserInfoEndpoint()
        {
            return Ok();
        }

        [HttpGet]
        [Route("mfa/challenge")]
        public IActionResult MfaChallengeEndpoint()
        {
            return Ok();
        }

        [HttpGet]
        [Route("oidc/register")]
        public IActionResult RegistrationEndpoint()
        {
            return Ok();
        }

        [HttpGet]
        [Route("oauth/revoke")]
        public IActionResult RevocationEndpoint()
        {
            return Ok();
        }

        [HttpGet]
        [Route("v2/logout")]
        public IActionResult Logout([FromQuery(Name = "client_id")] string clientId, [FromQuery] string returnTo)
        {
            var haveClientId = _organizations.Any(x => x.ClientId == clientId);
            if (!haveClientId)
            {
                return BadRequest("No loaded organization has this client_id!");
            }

            return RedirectPermanent(returnTo);
        }
    }
}
