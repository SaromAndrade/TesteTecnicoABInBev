using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomer
{
    public class GetSalesByCustomerQuery : IRequest<GetSalesByCustomerResult>
    { 
        public string CustomerId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Order { get; set; }

    }
}
