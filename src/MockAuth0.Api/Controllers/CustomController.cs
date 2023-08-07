using Microsoft.AspNetCore.Mvc;
using MockAuth0.Api.Models;
using MockAuth0.Api.Services;

namespace MockAuth0.Api.Controllers
{
    [ApiController]
    public class CustomController : Controller
    {
        private readonly List<OrganizationConfigurationModel> _organizations;
        private readonly IJwtGeneratorService _jwtGeneratorService;

        public CustomController(List<OrganizationConfigurationModel> organizations, IJwtGeneratorService jwtGeneratorService)
        {
            _organizations = organizations;
            _jwtGeneratorService = jwtGeneratorService;
        }

        [HttpPost, Route("makeAuthorization")]
        public IActionResult AuthorizationEndpont([FromForm] CustomAuthorizationFormModel model)
        {
            var currentOrganization = _organizations.FirstOrDefault(x => x.Id == model.OrganizationId);
            if (currentOrganization == null)
            {
                return BadRequest("Organization not found!");
            }
            if (currentOrganization.ClientId != model.ClientId)
            {
                return BadRequest("ClientId not found!");
            }

            var claimList = new List<KeyValuePair<string, string>>();
            foreach (var claim in currentOrganization.Claims)
            {
                var field = Request.Form[claim.ShortName];
                if (string.IsNullOrEmpty(field))
                {
                    claimList.Add(new(claim.FullName, claim.DefaultValue));
                }
                else
                {
                    claimList.Add(new(claim.FullName, field));
                }
            }




            var token = _jwtGeneratorService.GenerateToken(model.Email, model.ClientId, model.OrganizationId, model.Nonce, claimList);

            var file = System.IO.File.ReadAllText("forms/redirectForm.html");

            file = file
                .Replace("{{callback}}", currentOrganization.CallbackUriForLogin)
                .Replace("{{jwtToken}}", token)
                .Replace("{{state}}", model.State);

            return Content(file, "text/html");
        }

        [HttpGet, Route("health")]
        public IActionResult Health()
        {
            return Ok("Up and running");
        }
    }
}
