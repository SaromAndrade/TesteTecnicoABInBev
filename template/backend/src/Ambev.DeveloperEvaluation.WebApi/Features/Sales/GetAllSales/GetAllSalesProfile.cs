using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales
{
    public class GetAllSalesProfile : Profile
    {
        public GetAllSalesProfile()
        {
            CreateMap<GetAllSalesRequest, GetAllSalesQuery>();
            CreateMap<GetAllSalesResult, GetAllSalesResponse>();
        }
    }
}
