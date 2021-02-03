using AutoMapper;
using BankTransactionConciliationAPI.Models.Request;
using BankTransactionConciliationAPI.Models.Response;

namespace BankTransactionConciliationAPI.Models.Mapper
{
    public static class GeneralMapper
    {
        public static MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CreateBankTransactionRequest, BankTransaction>();
                config.CreateMap<BankTransaction, GetBankTransactionResponse>();
                config.CreateMap<ExportBankTransactionsToCsvRequest, BankTransactionFilters>();
                config.CreateMap<ListBankTransactionRequest, SearchPaging<BankTransactionFilters>>()
                    .ForPath(dest => dest.Filters.StartDate, opt => opt.MapFrom(src => src.StartDate))
                    .ForPath(dest => dest.Filters.EndDate, opt => opt.MapFrom(src => src.EndDate));
                config.CreateMap<SearchResult<BankTransaction>, PagingContainerResponse<GetBankTransactionResponse>>()
                    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Documents))
                    .AfterMap((src, dest) =>
                    {
                        dest.Paging = new PagingResponse
                        {
                            Page = src.Page,
                            Size = src.Size,
                            TotalItems = src.Count,
                            TotalPages = (int)((src.Count + src.Size - 1) / src.Size)
                        };
                    });
            });
        }
    }
}
