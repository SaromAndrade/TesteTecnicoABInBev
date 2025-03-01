using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProduct
{
    public class GetAllProductResponse
    {
        public List<ProductDto> Data { get; set; }
        public int TotalItems { get; set; }

    }
}
