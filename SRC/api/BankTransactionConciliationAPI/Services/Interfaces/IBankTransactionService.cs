using BankTransactionConciliationAPI.Models;
using System.Collections.Generic;

namespace BankTransactionConciliationAPI.Services.Interfaces
{
    public interface IBankTransactionService
    {
        List<BankTransaction> CreateMany(string ofx);

        SearchResult<BankTransaction> Search(SearchPaging<BankTransactionFilters> bankTransactionFilters);

        string ExportToCsv(BankTransactionFilters bankTransactionFilters);
    }
}
