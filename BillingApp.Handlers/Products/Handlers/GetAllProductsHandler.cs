
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Products.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Handlers.Products.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductDTO>>
    {
        private readonly BillingDbContext _context;

        public GetAllProductsHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Subcategory) // Requires Microsoft.EntityFrameworkCore
                .ThenInclude(s => s.Category)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SubcategoryId = p.SubcategoryId,
                    SubcategoryName = p.Subcategory.Name,  // Fetch subcategory name
                    CategoryName = p.Subcategory.Category.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
