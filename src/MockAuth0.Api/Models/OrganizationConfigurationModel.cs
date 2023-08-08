namespace MockAuth0.Api.Models
{
    public class OrganizationConfigurationModel
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string ClientId { get; set; }

        public string CallbackUriForLogin { get; set; }

        public string RedirectUriForLogin { get; set; }

        public string RedirectUriForLogout { get; set; }

        public List<OrganizationConfigurationClaimModel> Claims { get; set; }
    }

    public class OrganizationConfigurationClaimModel
    {
        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string DefaultValue { get; set; }

        public bool IsPrincipal { get; set; }
    }
}
