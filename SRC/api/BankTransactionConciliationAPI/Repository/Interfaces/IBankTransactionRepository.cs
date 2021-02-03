using BankTransactionConciliationAPI.Models;

namespace BankTransactionConciliationAPI.Repository.Interfaces
{
    public interface IBankTransactionRepository
    {
        void Upsert(BankTransaction bankTransaction);

        SearchResult<BankTransaction> Search(SearchPaging<BankTransactionFilters> bankTransactionFilters);
    }
}
