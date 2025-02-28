using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct
{
    public class DeleteProductProfile : Profile
    {
        public DeleteProductProfile()
        {
            CreateMap<DeleteProductRequest, DeleteProductCommand>();
        }
    }
}
