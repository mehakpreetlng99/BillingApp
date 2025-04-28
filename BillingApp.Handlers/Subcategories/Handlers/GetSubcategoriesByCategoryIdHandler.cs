using MediatR;
using BillingApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillingApp.DTO;
using BillingApp.Handlers.Subcategories.Queries;

namespace BillingApp.Handlers.Subcategories.Handlers
{
    public class GetSubcategoriesByCategoryIdHandler : IRequestHandler<GetSubcategoriesByCategoryIdQuery, List<SubcategoryDTO>>
    {
        private readonly BillingDbContext _context;

        public GetSubcategoriesByCategoryIdHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubcategoryDTO>> Handle(GetSubcategoriesByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var subcategories = await _context.Subcategories
                .Where(s => s.CategoryId == request.CategoryId)
                .Select(s => new SubcategoryDTO
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync(cancellationToken);

            return subcategories;
        }
    }
}
