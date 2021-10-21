using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Enums
{
    public enum TransactionType
    {
        Unknown = 0,
        TopUp = 1,
        Send = 2,
        Receive = 3,
        Expenses = 4,
        OtherIncome = 5,
        BuySecurity = 6,
        SellSecurity = 7
    }
}
