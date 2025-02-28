using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct
{
    public class GetAllProductQuery : IRequest<GetAllProductResult>
    {
        public int Page { get; set; } = 1; 
        public int Size { get; set; } = 10;
        public string Order { get; set; } = string.Empty;
    }
}
