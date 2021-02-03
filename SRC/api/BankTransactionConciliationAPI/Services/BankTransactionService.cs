using BankTransactionConciliationAPI.Models;
using BankTransactionConciliationAPI.Parsers.Interfaces;
using BankTransactionConciliationAPI.Repository.Interfaces;
using BankTransactionConciliationAPI.Services.Interfaces;
using System.Collections.Generic;

namespace BankTransactionConciliationAPI.Services
{
    public class BankTransactionService : IBankTransactionService
    {
        private readonly IBankTransactionRepository BankTransactionRepository;

        private readonly ICsvParser CsvParser;

        private readonly IOfxParser OfxParser;

        public BankTransactionService(
            IBankTransactionRepository bankTransactionRepository,
            ICsvParser csvParser,
            IOfxParser ofxParser)
        {
            this.BankTransactionRepository = bankTransactionRepository;
            this.CsvParser = csvParser;
            this.OfxParser = ofxParser;
        }

        public SearchResult<BankTransaction> Search(SearchPaging<BankTransactionFilters> bankTransactionFilters)
        {
            return this.BankTransactionRepository.Search(bankTransactionFilters);
        }

        public List<BankTransaction> CreateMany(string ofx)
        {
            var bankTransactions = this.OfxParser.Convert(ofx);

            foreach(var bankTransaction in bankTransactions)
            {
                this.BankTransactionRepository.Upsert(bankTransaction);
            }

            return bankTransactions;
        }

        public string ExportToCsv(BankTransactionFilters bankTransactionFilters)
        {
            var filters = new SearchPaging<BankTransactionFilters>
            {
                Page = 1,
                Size = 50,
                Filters = bankTransactionFilters
            };

            var bankTransactions = new List<BankTransaction>();
            var hasNext = false;
            
            do
            { 
                var result = this.BankTransactionRepository.Search(filters);
                bankTransactions.AddRange(result.Documents);
                hasNext = (result.Documents.Count == filters.Size);
                filters.Page++;
            }
            while (hasNext);

            return this.CsvParser.Convert(bankTransactions);
        }
    }
}
