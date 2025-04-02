
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Products.Queries
{

    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }

}
