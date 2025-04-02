using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.Handlers.Subcategories.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Subcategories.Handlers
{
    public class AddSubcategoryHandler : IRequestHandler<AddSubcategoryCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<AddSubcategoryHandler> _logger;

        public AddSubcategoryHandler(BillingDbContext context, ILogger<AddSubcategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(AddSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var subcategory = new Subcategory
            {
                Name = request.Subcategory.Name,
                CategoryId = request.Subcategory.CategoryId
            };

            await _context.Subcategories.AddAsync(subcategory, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                _logger.LogInformation($"Subcategory '{subcategory.Name}' added successfully.");
            else
                _logger.LogWarning($"Failed to add subcategory '{subcategory.Name}'.");

            return result;
        }
    }
}
