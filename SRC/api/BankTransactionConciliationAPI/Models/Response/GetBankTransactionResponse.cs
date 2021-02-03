using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionConciliationAPI.Models.Response
{
    public class GetBankTransactionResponse
    {
        public string Id { get; set; }

        public string Bank { get; set; }

        public string AccountNumber { get; set; }

        public string AccountType { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}
