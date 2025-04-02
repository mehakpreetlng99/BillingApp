
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Products.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Handlers.Products.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly BillingDbContext _context;

        public GetProductByIdHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.Subcategory)
                .ThenInclude(s => s.Category)  // Ensure category data is loaded
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return null;
            }

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                SubcategoryId = product.SubcategoryId,
                SubcategoryName = product.Subcategory.Name,
                CategoryName = product.Subcategory.Category.Name
            };
        }
    }
}
