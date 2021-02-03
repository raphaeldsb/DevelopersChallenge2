namespace BankTransactionConciliationAPI.Models.Request
{
    public class PagingRequest
    {
        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10;
    }
}
