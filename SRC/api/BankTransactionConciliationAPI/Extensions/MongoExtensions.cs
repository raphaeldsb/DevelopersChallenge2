using BankTransactionConciliationAPI.Models;
using MongoDB.Driver;

namespace BankTransactionConciliationAPI.Helpers
{
    public static class MongoExtensions
    {
        public static FindOptions<TDocument, TDocument> CreateFindOptions<TDocument, TFilter>(
            this SearchPaging<TFilter> paging, 
            SortDefinition<TDocument> sort)
        {
            var findOptions = new FindOptions<TDocument>();

            var offset = (paging.Page - 1) * paging.Size;
            findOptions.Skip = offset;
            findOptions.Limit = paging.Size;
            findOptions.Sort = sort;

            return findOptions;
        }
    }
}
