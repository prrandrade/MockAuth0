namespace MockAuth0.Api.Models
{
    public class DatabaseOrganizationModel
    {
        public int Id { get; set; }

        public string Clientid { get; set; }

        public string OrganizationId { get; set; }

        public Dictionary<string, string> ClaimsAndValues { get; set; } = new();
    }

    
}

