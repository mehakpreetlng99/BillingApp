using BillingApp.Data;
using BillingApp.Handlers.Categories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BillingApp.Handlers.Categories.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(BillingDbContext context, ILogger<DeleteCategoryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories.FindAsync(request.Id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {request.Id} not found.");
                    return false;
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Category with ID {request.Id} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting category: {ex.Message}");
                return false;
            }
        }
    }
}
