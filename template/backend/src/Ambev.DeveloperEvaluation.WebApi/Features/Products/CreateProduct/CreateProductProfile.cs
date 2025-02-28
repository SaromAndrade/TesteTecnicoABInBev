using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {
            CreateMap<CreateProductRequest, CreateProductCommand>();
            CreateMap<Rating, RatingDto>();
            CreateMap<RatingDto, RatingResponse>();
            CreateMap<CreateProductResult, CreateProductResponse>();
        }
    }
}
