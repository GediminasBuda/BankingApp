using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        

        public AccountController(
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<AccountBalanceResponse>> GetBalance(Guid id)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            var user = await _userRepository.GetAsync(firebaseId);

            var account = await _accountRepository.GetAsync(id);
            var transactions = await _transactionRepository.GetByAccountIdAsync(account.Id);
            

            var transactionsResponse = transactions.Select(transaction => new TransactionResponse
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                Comment = transaction.Comment,
                DateCreated = transaction.DateCreated
            });

            return Ok(new AccountBalanceResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance,
                Bankname = account.Bankname,
                Currency = account.Currency,
                DateCreated = account.DateCreated,
                Transactions = transactionsResponse
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateAccountResponse>> CreateAccount(AccountRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;

            var user = await _userRepository.GetAsync(firebaseId);

            var account = new AccountWriteModel
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                Balance = 0,
                Bankname = request.Bankname,
                Currency = request.Currency,
                DateCreated = DateTime.Now
            };

            await _accountRepository.SaveOrUpdateAsync(account);

            return Ok(new CreateAccountResponse
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance,
                Bankname = account.Bankname,
                Currency = account.Currency,
                DateCreated = account.DateCreated
            });
        }
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAccount(Guid id)
        {
            await _accountRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
