using System;

namespace BankTransactionConciliationAPI.Models
{
    public class BankTransactionFilters
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
