using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountReadModel>> GetAllAsync(Guid userId);

        Task<AccountReadModel> GetAsync(Guid id);

        Task<int> SaveOrUpdateAsync(AccountWriteModel model);

        Task<int> DeleteAsync(Guid id);
    }
}
