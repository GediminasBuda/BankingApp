using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models.ResponseModels
{
     public class AccountBalanceResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string Bankname { get; set; }
        public Currency Currency { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<TransactionResponse> Transactions { get; set; }
    }
}
