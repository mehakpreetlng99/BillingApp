
using BillingApp.Data;
using BillingApp.Handlers.Products.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Products.Handlers
{

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(BillingDbContext context, ILogger<UpdateProductHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Product.Id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {request.Product.Id} not found.");
                return false;
            }

            product.Name = request.Product.Name;
            product.Price = request.Product.Price;
            product.Quantity = request.Product.Quantity;
            product.SubcategoryId = request.Product.SubcategoryId;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                _logger.LogInformation($"Product '{product.Name}' updated successfully.");

            return result;
        }
    }
}
