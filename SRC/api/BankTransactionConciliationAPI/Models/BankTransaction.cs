using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BankTransactionConciliationAPI.Models
{
    public class BankTransaction
    {
        public BankTransaction()
        {
            this.Id = GenerateId();
        }

        public BankTransaction(
            string bank,
            string accountNumber,
            string accountType,
            DateTime transactionDate,
            string transactionType,
            decimal amount,
            string description
            )
        {
            this.Bank = bank;
            this.AccountNumber = accountNumber;
            this.AccountType = accountType;
            this.TransactionDate = transactionDate;
            this.TransactionType = transactionType;
            this.Amount = amount;
            this.Description = description;
            this.Id = GenerateId();
        }

        [BsonId]
        public string Id { get; private set; }

        public string Bank { get; private set; }

        public string AccountNumber { get; private set; }

        public string AccountType { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public string TransactionType { get; private set; }

        public decimal Amount { get; private set; }

        public string Description { get; private set; }

        private string GenerateId()
        {
            var text = $"{this.Bank}{this.AccountNumber}{this.AccountType}{this.TransactionDate}" + 
                       $"{this.TransactionType}{this.Amount}{this.Description}";

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var sha1 = SHA1.Create();
            var hash = BitConverter.ToString(sha1.ComputeHash(buffer));

            return hash.Replace("-", "");
        }
    }
}
