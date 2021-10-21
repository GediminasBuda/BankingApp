using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using System;
using Contracts.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Domain.Clients.Firebase.Models;

namespace Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<TransactionResponse> TopUp(TransactionRequest request, string firebaseId)
        {
            var user = await _userRepository.GetAsync(firebaseId);
            var account = await _accountRepository.GetAsync(request.AccountId);

            var transactionWriteModels = new TransactionWriteModel
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                AccountId = request.AccountId,
                TransactionType = request.TransactionType,
                Amount = request.Amount,
                Comment = request.Comment,
                DateCreated = DateTime.Now
            };

            var accountWriteModels = new AccountWriteModel
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance + request.Amount,
                Bankname = account.Bankname,
                Currency = account.Currency,
                DateCreated = account.DateCreated
            };

            await _transactionRepository.SaveOrUpdateAsync(transactionWriteModels);
            await _accountRepository.SaveOrUpdateAsync(accountWriteModels);

            return new TransactionResponse
            {
                Id = transactionWriteModels.Id,
                UserId = transactionWriteModels.UserId,
                TransactionType = transactionWriteModels.TransactionType,
                Amount = transactionWriteModels.Amount,
                Comment = transactionWriteModels.Comment,
                DateCreated = transactionWriteModels.DateCreated
            };
        }
        public Task<TransactionResponse> Receive(TransactionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionResponse> Send(TransactionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
