using BankTransactionConciliationAPI.Models;
using System.Collections.Generic;

namespace BankTransactionConciliationAPI.Parsers.Interfaces
{
    public interface IOfxParser
    {
        List<BankTransaction> Convert(string ofx);
    }
}
