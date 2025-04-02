using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Subcategories.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Subcategories.Handlers
{
    public class GetAllSubcategoriesHandler : IRequestHandler<GetSubcategoriesQuery, List<SubcategoryDTO>>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<GetAllSubcategoriesHandler> _logger;

        public GetAllSubcategoriesHandler(BillingDbContext context, ILogger<GetAllSubcategoriesHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<SubcategoryDTO>> Handle(GetSubcategoriesQuery request, CancellationToken cancellationToken)
        {
            var subcategories = await _context.Subcategories
                .Select(s => new SubcategoryDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation($"Retrieved {subcategories.Count} subcategories.");
            return subcategories;
        }
    }
}
