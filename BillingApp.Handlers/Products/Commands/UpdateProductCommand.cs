
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Products.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public ProductDTO Product { get; }

        public UpdateProductCommand(ProductDTO product)
        {
            Product = product;
        }
    }
}
