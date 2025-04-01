using BillingApp.Data;
using BillingApp.Handlers.Categories.Commands;
using BillingApp.Models;
using System;
using System.Threading;
using System.Threading.Tasks;



using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Categories.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, bool>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(BillingDbContext context, ILogger<CreateCategoryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = new BillingApp.Models.Category
                {
                    Name = request.Name
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Category '{request.Name}' created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating category: {ex.Message}");
                return false;
            }
        }
    }
}
