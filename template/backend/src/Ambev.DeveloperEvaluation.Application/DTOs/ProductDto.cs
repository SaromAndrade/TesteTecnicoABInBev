using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; } // ID do produto
        public string Title { get; set; } // Título do produto
        public decimal Price { get; set; } // Preço do produto
        public string Description { get; set; } // Descrição do produto
        public string Category { get; set; } // Categoria do produto
        public string Image { get; set; } // URL da imagem do produto
        public RatingDto Rating { get; set; } // Avaliação do produto
    }
}
