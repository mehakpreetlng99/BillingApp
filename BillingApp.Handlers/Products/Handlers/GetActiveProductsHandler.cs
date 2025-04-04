using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Products.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BillingApp.Handlers.Products.Handlers
{
    public class GetActiveProductsHandler : IRequestHandler<GetActiveProductsQuery, List<ProductDTO>>
    {
        private readonly BillingDbContext _context;

        public GetActiveProductsHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(p => p.IsActive != false) 
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SubcategoryId = p.SubcategoryId
                })
                .ToListAsync(cancellationToken);
        }
    }
}