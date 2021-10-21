using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ISqlClient _sqlClient;
        private const string TableName = "users";

        public UserRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<UserReadModel> GetAsync(string firebaseId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE FirebaseId = @FirebaseId";
            return _sqlClient.QuerySingleOrDefaultAsync<UserReadModel>(sql, new
            {
                FirebaseId = firebaseId
            });

        }

        public Task<int> SaveAsync(UserWriteModel model)
        {
            var sql = @$"INSERT INTO {TableName} (UserId, FirebaseId, Username, Email, DateCreated)
                VALUES (@UserId, @FirebaseId, @Username, @Email, @DateCreated)";
            return _sqlClient.ExecuteAsync(sql, model);
        }
    }
}
