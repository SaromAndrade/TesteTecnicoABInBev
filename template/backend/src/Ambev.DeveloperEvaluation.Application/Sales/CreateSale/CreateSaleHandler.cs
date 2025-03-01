using Ambev.DeveloperEvaluation.Domain;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly DiscountService _discountService;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;   
            _productRepository = productRepository;
            _discountService = new DiscountService();
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator(_productRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = _mapper.Map<Sale>(request);

            decimal totalSaleAmount = 0;
            decimal totalDiscount = 0;
            decimal totalWithDiscount = 0;
            foreach (var product in sale.Products)
            {
                if (product.Quantity > 20)
                    throw new ArgumentException($"Produto {product.ProductId} não pode ter mais de 20 unidades por venda.");

                decimal discount = _discountService.GetDiscount(product.Quantity, product.UnitPrice);
                decimal totalItemAmount = product.Quantity * product.UnitPrice;
                decimal totalItemWithDiscount = totalItemAmount - discount;

                product.DiscountAmount = discount;
                product.TotalItemAmount = totalItemAmount;
                product.FinalItemAmount = totalItemWithDiscount;

                totalSaleAmount += totalItemAmount;
                totalDiscount += discount;
                totalWithDiscount += totalItemWithDiscount;
            }

            sale.TotalAmount = totalSaleAmount;
            sale.DiscountAmount = totalDiscount;
            sale.FinalAmount = totalWithDiscount;
            sale.Date = DateTime.UtcNow;

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            var result = _mapper.Map<CreateSaleResult>(createdSale);

            return result;
        }
    }
}
