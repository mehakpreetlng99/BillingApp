using BillingApp.Data;
using BillingApp.Handlers.Categories.Queries;
using BillingApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BillingApp.Handlers.Categories.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<GetCategoriesQueryHandler> _logger;

        public GetCategoriesQueryHandler(BillingDbContext context, ILogger<GetCategoriesQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _context.Categories.ToListAsync(cancellationToken);

                if (categories.Count == 0)
                {
                    _logger.LogWarning("No categories found.");
                }

                _logger.LogInformation($"Retrieved {categories.Count} categories.");
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving categories: {ex.Message}");
                return new List<Category>(); 
            }
        }
    }
}
