using System;

namespace BankTransactionConciliationAPI.Models.Request
{
    public class ExportBankTransactionsToCsvRequest
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
