using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Products.Queries
{
    public record GetActiveProductsQuery : IRequest<List<ProductDTO>>;
}
