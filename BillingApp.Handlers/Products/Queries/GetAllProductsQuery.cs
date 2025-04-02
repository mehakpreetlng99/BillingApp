
using MediatR;
using BillingApp.DTO;

namespace BillingApp.Handlers.Products.Queries
{
    public class GetAllProductsQuery : IRequest<List<ProductDTO>> { }
}
