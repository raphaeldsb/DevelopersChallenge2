using BankTransactionConciliationAPI.Models;
using System.Collections.Generic;

namespace BankTransactionConciliationAPI.Parsers.Interfaces
{
    public interface ICsvParser
    {
        string Convert(List<BankTransaction> bankTransactions);
    }
}
