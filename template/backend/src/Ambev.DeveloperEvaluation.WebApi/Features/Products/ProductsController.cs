using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="request">The user creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created user details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateProductCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "User created successfully",
                Data = _mapper.Map<CreateProductResponse>(response)
            });
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<GetAllProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts([FromQuery] int page, [FromQuery] int size, [FromQuery] string order, CancellationToken cancellationToken)
        {
            var query = new GetAllProductQuery { Size = size, Order = order, Page = page };
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetAllProductResponse>(result);

            var pagedList = new PaginatedList<ProductDto>(response.Data, response.TotalItems, response.CurrentPage, size);

            // Retorna os dados paginados
            return Ok(new PaginatedResponse<ProductDto>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });
        }
    }
}
