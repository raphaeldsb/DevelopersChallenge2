namespace BankTransactionConciliationAPI.Models
{
    public class SearchPaging<T>
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public T Filters { get; set; }
    }
}
