using System;

namespace BankTransactionConciliationAPI.Models.Request
{
    public class ListBankTransactionRequest : PagingRequest
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
