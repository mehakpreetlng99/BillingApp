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
    public class DeleteSubcategoryHandler : IRequestHandler<DeleteSubcategoryCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<DeleteSubcategoryHandler> _logger;

        public DeleteSubcategoryHandler(BillingDbContext context, ILogger<DeleteSubcategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var subcategory = await _context.Subcategories.FindAsync(request.Id);

            if (subcategory == null)
            {
                _logger.LogWarning($"Subcategory with ID {request.Id} not found.");
                return false;
            }

            _context.Subcategories.Remove(subcategory);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                _logger.LogInformation($"Subcategory '{subcategory.Name}' deleted successfully.");
            else
                _logger.LogWarning($"Failed to delete subcategory '{subcategory.Name}'.");

            return result;
        }
    }
}
