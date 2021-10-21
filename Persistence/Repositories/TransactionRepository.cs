using Contracts.Enums;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ISqlClient _sqlClient;
        private const string TableName = "transactions";
        public TransactionRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<IEnumerable<TransactionReadModel>> GetAllAsync(Guid userId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE UserId = @UserId";

            return _sqlClient.QueryAsync<TransactionReadModel>(sql, new
            {
                UserId = userId
            });
        }
        public Task<IEnumerable<TransactionReadModel>> GetByAccountIdAsync(Guid accountId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE AccountId = @AccountId";

            return _sqlClient.QueryAsync<TransactionReadModel>(sql, new
            {
                AccountId = accountId
            });
        }
        public Task<TransactionReadModel> GetAsync(Guid id)
        {
            var sql = $"SELECT FROM {TableName} WHERE Id = @Id";

            return _sqlClient.QuerySingleOrDefaultAsync<TransactionReadModel>(sql, new
            {
                Id = id
            });
        }
        public Task<IEnumerable<TransactionReadModel>> GetAsync(TransactionType transactionType, Guid userId)
        {
            var sql = $"SELECT FROM {TableName} WHERE Id = @Id AND UserId = @UserId";

            return _sqlClient.QueryAsync<TransactionReadModel>(sql, new
            {
                UserId = userId,
                TransactionType = transactionType
            });
        }

        public Task<int> SaveOrUpdateAsync(TransactionWriteModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, UserId, AccountId, TransactionType, Amount, Comment, DateCreated) 
                        VALUES (@Id, @UserId, @AccountId, @TransactionType, @Amount, @Comment, @DateCreated)
                        ON DUPLICATE KEY UPDATE Amount = @Amount, Comment = @Comment";

            return _sqlClient.ExecuteAsync(sql, new
            {
                model.Id,
                model.UserId,
                model.AccountId,
                TransactionType = model.TransactionType.ToString(),
                model.Amount,
                model.Comment,
                model.DateCreated
            });
        }
        public Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @Id";

            return _sqlClient.ExecuteAsync(sql, new
            {
                Id = id
            });
        }

        public Task<int> DeleteByAccountIdAsync(Guid accountId)
        {
            var sql = $"DELETE FROM {TableName} WHERE AccountId = @AccountId";

            return _sqlClient.ExecuteAsync(sql, new
            {
                AccountId = accountId
            });
        }

    }
}
