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
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionReadModel>> GetAllAsync(Guid userId);

        Task<IEnumerable<TransactionReadModel>> GetByAccountIdAsync(Guid accountId);

        Task<TransactionReadModel> GetAsync(Guid id);

        Task<IEnumerable<TransactionReadModel>> GetAsync(TransactionType transactionType, Guid userId);

        Task<int> SaveOrUpdateAsync(TransactionWriteModel model);

        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteByAccountIdAsync(Guid accountId);
    }
}
