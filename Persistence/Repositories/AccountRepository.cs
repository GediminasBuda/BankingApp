using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private const string TableName = "accounts";
        private readonly ISqlClient _sqlClient;

        public AccountRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<IEnumerable<AccountReadModel>> GetAllAsync(Guid userId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE UserId = @UserId";

            return _sqlClient.QueryAsync<AccountReadModel>(sql, new { UserId = userId });
        }
      
        public Task<AccountReadModel> GetAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";

            return _sqlClient.QuerySingleOrDefaultAsync<AccountReadModel>(sql, new { Id = id });
        }

        public Task<int> SaveOrUpdateAsync(AccountWriteModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, UserId, Balance, Bankname, Currency, DateCreated) 
                        VALUES (@Id, @UserId, @Balance, @Bankname, @Currency, @DateCreated)
                        ON DUPLICATE KEY UPDATE Balance = @Balance, Bankname = @Bankname, Currency = @Currency";

            return _sqlClient.ExecuteAsync(sql, new
            {
                model.Id,
                model.UserId,
                model.Balance,
                model.Bankname,
                Currency = model.Currency.ToString(),
                model.DateCreated
            });
        }
        public Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE from {TableName} WHERE Id = @Id";
            return _sqlClient.ExecuteAsync(sql, new { Id = id });
        }
    }
}
