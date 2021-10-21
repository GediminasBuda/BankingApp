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
using Contracts.Models;

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
            if (account.Balance + request.Amount < 0)
            {
                throw new Exception($"Insufficient funds in the account!");
            }
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

        public async Task<SendTransactionResponse> Send(SendTransactionRequest request, string firebaseId)
        {
            var user = await _userRepository.GetAsync(firebaseId);
            var account = await _accountRepository.GetAsync(request.AccountId);
            var receiverAccount = await _accountRepository.GetAsync(request.ReceiverSenderAccountId);

            if(account.Balance < request.Amount)
            {
                throw new Exception($"Insufficient funds in the account!");
            }
            if (account.Currency != receiverAccount.Currency)
            {
                throw new Exception($"Receiver's account's currency does not match!");
            }
            var senderTransactionWriteModels = new TransactionWriteModel
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                AccountId = request.AccountId,
                ReceiverSenderAccountId = receiverAccount.Id,
                TransactionType = TransactionType.Send,
                Amount = - request.Amount,
                Comment = request.Comment,
                DateCreated = DateTime.Now
            };
            var receiverTransactionWriteModels = new TransactionWriteModel
            {
                Id = Guid.NewGuid(),
                UserId = receiverAccount.UserId,
                AccountId = receiverAccount.Id,
                ReceiverSenderAccountId = request.AccountId,
                TransactionType = TransactionType.Receive,
                Amount = request.Amount,
                Comment = request.Comment,
                DateCreated = DateTime.Now
            };

            var accountWriteModels = new AccountWriteModel
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance - request.Amount,
                Bankname = account.Bankname,
                Currency = account.Currency,
                DateCreated = account.DateCreated
            };
            var receiverAccountWriteModels = new AccountWriteModel
            {
                Id = receiverAccount.Id,
                UserId = receiverAccount.UserId,
                Balance = receiverAccount.Balance + request.Amount,
                Bankname = receiverAccount.Bankname,
                Currency = receiverAccount.Currency,
                DateCreated = receiverAccount.DateCreated
            };

            await _transactionRepository.SaveOrUpdateAsync(senderTransactionWriteModels);
            await _transactionRepository.SaveOrUpdateAsync(receiverTransactionWriteModels);
            await _accountRepository.SaveOrUpdateAsync(accountWriteModels);
            await _accountRepository.SaveOrUpdateAsync(receiverAccountWriteModels);

            return new SendTransactionResponse
            {
                Id = senderTransactionWriteModels.Id,
                UserId = senderTransactionWriteModels.UserId,
                ReceiverSenderAccountId = senderTransactionWriteModels.ReceiverSenderAccountId,
                TransactionType = senderTransactionWriteModels.TransactionType,
                Amount = senderTransactionWriteModels.Amount,
                Comment = senderTransactionWriteModels.Comment,
                DateCreated = senderTransactionWriteModels.DateCreated
            };
        }
    }
}
