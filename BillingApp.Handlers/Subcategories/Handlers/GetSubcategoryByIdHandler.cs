using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Subcategories.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Subcategories.Handlers
{
    public class GetSubcategoryByIdHandler : IRequestHandler<GetSubcategoryByIdQuery, SubcategoryDTO>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<GetSubcategoryByIdHandler> _logger;

        public GetSubcategoryByIdHandler(BillingDbContext context, ILogger<GetSubcategoryByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SubcategoryDTO> Handle(GetSubcategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var subcategory = await _context.Subcategories
                .Where(s => s.Id == request.Id)
                .Select(s => new SubcategoryDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (subcategory == null)
                _logger.LogWarning($"Subcategory with ID {request.Id} not found.");
            else
                _logger.LogInformation($"Retrieved Subcategory: {subcategory.Name}");

            return subcategory;
        }
    }
}
