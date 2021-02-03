using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionConciliationAPI.Models.Response
{
    public class PagingContainerResponse<T>
    {
        public PagingResponse Paging { get; set; }

        public List<T> Items { get; set; }
    }

    public class PagingResponse
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public long TotalItems { get; set; }

        public int TotalPages { get; set; }
    }
}
