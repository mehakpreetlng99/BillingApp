using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.Handlers.Subcategories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Subcategories.Handlers
{
    public class UpdateSubcategoryHandler : IRequestHandler<UpdateSubcategoryCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<UpdateSubcategoryHandler> _logger;

        public UpdateSubcategoryHandler(BillingDbContext context, ILogger<UpdateSubcategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var subcategory = await _context.Subcategories.FindAsync(request.Subcategory.Id);

            if (subcategory == null)
            {
                _logger.LogWarning($"Subcategory with ID {request.Subcategory.Id} not found.");
                return false;
            }

            subcategory.Name = request.Subcategory.Name;
            subcategory.CategoryId = request.Subcategory.CategoryId;

            _context.Subcategories.Update(subcategory);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                _logger.LogInformation($"Subcategory '{subcategory.Name}' updated successfully.");
            else
                _logger.LogWarning($"Failed to update subcategory '{subcategory.Name}'.");

            return result;
        }
    }
}
