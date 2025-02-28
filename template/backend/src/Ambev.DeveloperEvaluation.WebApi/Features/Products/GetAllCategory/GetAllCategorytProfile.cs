using Ambev.DeveloperEvaluation.Application.Products.GetAllCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllCategory
{
    public class GetAllCategorytProfile : Profile
    {
        public GetAllCategorytProfile()
        {
            CreateMap<GetAllCategorytResult, GetAllCategorytResponse>();
        }
    }
}
