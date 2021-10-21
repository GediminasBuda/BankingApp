using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models.RequestModels
{
    public class AccountRequest
    {
        public string Bankname { get; set; }
        public Currency Currency { get; set; }
    }
}
