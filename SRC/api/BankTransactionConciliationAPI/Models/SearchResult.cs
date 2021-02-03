using System.Collections.Generic;

namespace BankTransactionConciliationAPI.Models
{
    public class SearchResult<T>
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public long Count { get; set; }

        public List<T> Documents { get; set; }
    }
}
