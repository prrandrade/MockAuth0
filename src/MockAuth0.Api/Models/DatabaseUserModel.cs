namespace MockAuth0.Api.Models
{
	public class DatabaseUserModel
	{
		public int Id { get; set; }

		public string OrganizationId { get; set; }

		public string ClientId { get; set; }

		public Dictionary<string, string> ClaimsAndValues { get; set; }
	}
}
