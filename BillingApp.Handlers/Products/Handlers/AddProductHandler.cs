using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.Handlers.Products.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Products.Handlers
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<AddProductHandler> _logger;

        public AddProductHandler(BillingDbContext context, ILogger<AddProductHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Product.Name,
                Price = request.Product.Price,
                Quantity = request.Product.Quantity,
                SubcategoryId = request.Product.SubcategoryId
            };

            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                _logger.LogInformation($"Product '{product.Name}' added successfully.");

            return result;
        }
    }
}
