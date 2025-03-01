using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerProfile : Profile
    {
        public GetSalesByCustomerProfile()
        {
            CreateMap<GetSalesByCustomerRequest, GetSalesByCustomerQuery>();
            CreateMap<GetSalesByCustomerResult, GetSalesByCustomerResponse>();
        }
    }
}
