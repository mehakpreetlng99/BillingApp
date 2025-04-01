using BillingApp.Data;
using BillingApp.Handlers.Categories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Handlers.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(BillingDbContext context, ILogger<UpdateCategoryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories.FindAsync(request.Id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {request.Id} not found.");
                    return false;
                }

                category.Name = request.Name;
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Category with ID {request.Id} updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating category: {ex.Message}");
                return false;
            }
        }
    }
}
