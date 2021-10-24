using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models.RequestModels
{
    public class SendTransactionRequest
    {
        public Guid AccountId { get; set; }
        public Guid ReceiverSenderAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
