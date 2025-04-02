
using BillingApp.Data;
using BillingApp.Handlers.Products.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Products.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(BillingDbContext context, ILogger<DeleteProductHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {request.Id} not found.");
                return false;
            }

            _context.Products.Remove(product);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                _logger.LogInformation($"Product '{product.Name}' deleted successfully.");

            return result;
        }
    }
}
