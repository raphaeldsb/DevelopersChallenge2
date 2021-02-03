using BankTransactionConciliationAPI.Helpers;
using BankTransactionConciliationAPI.Models;
using BankTransactionConciliationAPI.Repository.Interfaces;
using MongoDB.Driver;
using System;

namespace BankTransactionConciliationAPI.Repository
{
    public class BankTransactionRepository : IBankTransactionRepository
    {
        private readonly IMongoCollection<BankTransaction> MongoCollection;

        public BankTransactionRepository(IMongoDatabase mongoDatabase)
        {
            this.MongoCollection = mongoDatabase.GetCollection<BankTransaction>(typeof(BankTransaction).Name);
        }

        public SearchResult<BankTransaction> Search(SearchPaging<BankTransactionFilters> bankTransactionFilters)
        {
            var filters = this.SearchBankTransactionFilterBuilder(bankTransactionFilters.Filters);
            var sort = Builders<BankTransaction>.Sort.Descending(r => r.TransactionDate);

            var options = bankTransactionFilters.CreateFindOptions(sort);

            var documents = this.MongoCollection.FindAsync(filters, options)
                .GetAwaiter().GetResult().ToList();

            var count = this.MongoCollection.CountDocuments(filters);

            return new SearchResult<BankTransaction>
            {
                Count = count,
                Page = bankTransactionFilters.Page,
                Size = bankTransactionFilters.Size,
                Documents = documents
            };
        }

        private FilterDefinition<BankTransaction> SearchBankTransactionFilterBuilder(BankTransactionFilters bankTransactionFilters)
        {
            var builder = Builders<BankTransaction>.Filter;
            
            if (bankTransactionFilters?.StartDate == null &&
                bankTransactionFilters?.EndDate == null)
            {
                return builder.Empty;
            }

            FilterDefinition<BankTransaction> filters = null;

            if (bankTransactionFilters.StartDate != null)
            {
                filters = builder.Gte(r => r.TransactionDate, bankTransactionFilters.StartDate.Value);
            }

            if (bankTransactionFilters.EndDate != null)
            {
                var filterByEndDate = builder.Lte(r => r.TransactionDate, bankTransactionFilters.EndDate.Value);
                filters = (filters == null) ? filterByEndDate : filters & filterByEndDate;
            }

            return filters;
        }

        public void Upsert(BankTransaction bankTransaction)
        {
            if (bankTransaction == null)
            {
                throw new ArgumentNullException(nameof(bankTransaction));
            }

            var filter = Builders<BankTransaction>.Filter.Eq("_id", bankTransaction.Id);
            var options = new ReplaceOptions { IsUpsert = true };

            this.MongoCollection.ReplaceOne(filter, bankTransaction, options);
        }
    }
}
