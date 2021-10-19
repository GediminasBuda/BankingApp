using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<UserReadModel> GetByIdAsync(string firebaseId);
        Task<int> SaveAsync(UserWriteModel model);
    }
}
