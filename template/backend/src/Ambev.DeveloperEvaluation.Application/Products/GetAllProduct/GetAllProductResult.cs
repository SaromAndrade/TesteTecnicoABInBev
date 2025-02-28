﻿using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct
{
    public class GetAllProductResult
    {
        public List<ProductDto> Data { get; set; } // Lista de produtos
        public int TotalItems { get; set; } // Total de itens
        public int CurrentPage { get; set; } // Página atual
        public int TotalPages { get; set; } // Total de páginas
    }
}
