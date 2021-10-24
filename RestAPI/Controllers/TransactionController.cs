using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Authorize]
        [Route("topUp")]
        public async Task<ActionResult<TransactionResponse>> TopUpAccount(TransactionRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            try
            {
                var response = await _transactionService.TopUpAsync(request, firebaseId);

                return response;
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("send")]
        public async Task<ActionResult<SendTransactionResponse>> Transfer(SendTransactionRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            try
            {
                var response = await _transactionService.SendAsync(request, firebaseId);

                return response;
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<TransactionsResponse>> GetAllTransactions()
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            try
            {
                var response = await _transactionService.GetAllAsync(firebaseId);

                return response;
            }
            catch (BadHttpRequestException e)
            {
                return (IEnumerable<TransactionsResponse>)BadRequest(e.Message);
            }
        }
    }
}
