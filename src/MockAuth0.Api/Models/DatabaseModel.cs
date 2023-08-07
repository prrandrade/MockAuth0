namespace MockAuth0.Api.Models
{
    public class DatabaseModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Clientid { get; set; }

        public string OrganizationId { get; set; }

        public Dictionary<string, string> ClaimsAndValues { get; set; } = new();
    }
}

