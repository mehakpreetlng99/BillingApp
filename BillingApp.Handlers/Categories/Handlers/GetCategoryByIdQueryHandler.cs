using BillingApp.Data;
using BillingApp.Handlers.Categories.Queries;
using BillingApp.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Handlers.Categories.Handlers
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<GetCategoryQueryHandler> _logger;

        public GetCategoryQueryHandler(BillingDbContext context, ILogger<GetCategoryQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories.FindAsync(request.Id);

                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {request.Id} not found.");
                    return null; // Or handle according to your need
                }

                _logger.LogInformation($"Category with ID {request.Id} found: {category.Name}");
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving category with ID {request.Id}: {ex.Message}");
                return null; // Or handle according to your need
            }
        }
    }
}
