using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct
{
    public class GetAllProductProfile : Profile
    {
        public GetAllProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Rating, RatingDto>();
        }
    }
}
