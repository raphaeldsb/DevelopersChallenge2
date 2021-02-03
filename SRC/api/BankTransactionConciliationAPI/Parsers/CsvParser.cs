using BankTransactionConciliationAPI.Models;
using BankTransactionConciliationAPI.Parsers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionConciliationAPI.Parsers
{
    public class CsvParser : ICsvParser
    {
        private const string SEPARATOR = ",";

        public string Convert(List<BankTransaction> bankTransactions)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(this.ToHeader());

            foreach (var item in bankTransactions)
            {
                stringBuilder.Append($"\n{this.ToCsvLine(item)}");
            }

            return stringBuilder.ToString();
        }

        private string ToHeader()
        {
            List<string> columns = new List<string>
            {
               "Bank",
               "Account_Number",
               "Account_Type",
               "Transaction_Date",
               "Transaction_Type",
               "Amount",
               "Description"
            };

            return string.Join(SEPARATOR, columns);
        }

        private string ToCsvLine(BankTransaction bankTransaction)
        {
            List<string> columns = new List<string>
            {
               bankTransaction.Bank,
               bankTransaction.AccountNumber,
               bankTransaction.AccountType,
               bankTransaction.TransactionDate.ToString(),
               bankTransaction.TransactionType,
               bankTransaction.Amount.ToString(),
               this.Normalize(bankTransaction.Description)
            };

            return string.Join(SEPARATOR, columns);
        }

        private string Normalize(string str)
        {
            return str.Replace(SEPARATOR, "");
        }
    }
}
