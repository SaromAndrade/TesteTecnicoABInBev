using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory
{
    public class GetProductsByCategoryResult
    {
        public List<ProductDto> Data { get; set; } // Lista de produtos
        public int TotalItems { get; set; } // Total de itens
        public int CurrentPage { get; set; } // Página atual
        public int TotalPages { get; set; } // Total de páginas
    }
}
