
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Products.Commands
{
    public class AddProductCommand : IRequest<bool>
    {
        public ProductDTO Product { get; }

        public AddProductCommand(ProductDTO product)
        {
            Product = product;
        }
    }
}
