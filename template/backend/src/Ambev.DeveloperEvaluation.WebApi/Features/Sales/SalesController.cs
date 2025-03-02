using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomer;
using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByDateRange;
using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByDateRange;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
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
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateSaleCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<CreateSaleResponse>(result);

            return Ok<CreateSaleResponse>(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<GetAllSalesRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSales([FromQuery] int page, [FromQuery] int size, [FromQuery] string order, CancellationToken cancellationToken)
        {
            var request = new GetAllSalesRequest { Size = size, Order = order, Page = page };
            var validator = new GetAllSalesRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var query = _mapper.Map<GetAllSalesQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetAllSalesResponse>(result);

            var pagedList = new PaginatedList<Sale>(response.Sales, response.TotalItems, page, size);

            return OkPaginated<Sale>(pagedList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSale([FromRoute] int id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var query = _mapper.Map<GetSaleQuery>(request);

            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetSaleResponse>(result);

            return Ok<GetSaleResponse>(response);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSale([FromRoute] int id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest("Invalid product ID.");

            var validator = new UpdateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<UpdateSaleCommand>(request);
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<UpdateSaleResponse>(result);

            return Ok<UpdateSaleResponse>(response);
        }
        [HttpPut("cancel/{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelSale([FromRoute] int id, CancellationToken cancellationToken)
        {
            var request = new CancelSaleRequest() { SaleNumber = id };

            var validator = new CancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CancelSaleCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok<string>(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id, CancellationToken cancellationToken)
        {
            var request = new DeleteSaleRequest { Id = id };
            var validator = new DeleteSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<DeleteSaleCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok<string>(result);
        }

        [HttpGet("customerId")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSalesByCustomerRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByCustomer([FromBody] GetSalesByCustomerRequest request , CancellationToken cancellationToken)
        {
            var validator = new GetSalesByCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var query = _mapper.Map<GetSalesByCustomerQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetSalesByCustomerResponse>(result);

            var pagedList = new PaginatedList<Sale>(response.Sales, response.TotalItems, request.Page, request.Size);

            return OkPaginated<Sale>(pagedList);
        }
        [HttpGet("daterange")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSalesByDateRangeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int page, [FromQuery] int size, [FromQuery] string order,  CancellationToken cancellationToken)
        {
            var request = new GetSalesByDateRangeRequest() { StartDate = startDate, EndDate = endDate, Size = size, Order = order, Page = page };
            var validator = new GetSalesByDateRangeRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var query = _mapper.Map<GetSalesByDateRangeQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetSalesByDateRangeResponse>(result);

            var pagedList = new PaginatedList<Sale>(response.Sales, response.TotalItems, request.Page, request.Size);

            return OkPaginated<Sale>(pagedList);
        }
    }
}
