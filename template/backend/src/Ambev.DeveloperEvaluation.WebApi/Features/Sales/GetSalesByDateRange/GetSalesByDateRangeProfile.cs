using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByDateRange
{
    public class GetSalesByDateRangeProfile : Profile
    {
        public GetSalesByDateRangeProfile()
        {
            CreateMap<GetSalesByDateRangeRequest, GetSalesByDateRangeQuery>();
            CreateMap<GetSalesByDateRangeResult, GetSalesByDateRangeResponse>();
        }
    }
}
