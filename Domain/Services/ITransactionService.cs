using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
using Domain.Clients.Firebase.Models;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse> TopUp(TransactionRequest request, string firebaseId);
        Task<SendTransactionResponse> Send(SendTransactionRequest request, string firebaseId);

        Task<TransactionResponse> Receive(TransactionRequest request);
        Task<IEnumerable<TransactionsResponse>> GetAllAsync(Guid userId, string firebaseId);

    }
}
