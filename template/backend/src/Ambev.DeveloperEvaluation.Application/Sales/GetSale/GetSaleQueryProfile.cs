using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleQueryProfile : Profile
    {
        public GetSaleQueryProfile()
        {
            CreateMap<Sale, GetSaleResult>();
        }
    }
}
