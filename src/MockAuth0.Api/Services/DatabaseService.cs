using LiteDB;
using MockAuth0.Api.Models;

namespace MockAuth0.Api.Services
{
    public interface IDatabaseService
    {
        DatabaseModel GetByEmailAndClientIdAndOrganizationId(string email, string clientId, string organizationId);

        void Save(DatabaseModel model);
    }


    public class DatabaseService : IDatabaseService
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<DatabaseModel> _col;

        public DatabaseService()
        {
            _database = new LiteDatabase(@"config\MyData.db");
            _col = _database.GetCollection<DatabaseModel>("database");
        }

        public DatabaseModel GetByEmailAndClientIdAndOrganizationId(string email, string clientId, string organizationId) => _col.FindOne(x => x.Email == email && x.Clientid == clientId && x.OrganizationId == organizationId);

        public void Save(DatabaseModel model) => _col.Upsert(model);
    }
}
