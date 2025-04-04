using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Invoices.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Handlers.Invoices.Handlers
{
    public class GetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDTO>
    {
        private readonly BillingDbContext _context;

        public GetInvoiceByIdHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                    .ThenInclude(ii => ii.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

            if (invoice == null)
            {
                return null;
            }

            var customerName = invoice.Customer?.Name ?? "Unknown Customer";
            var customerPhone = invoice.Customer?.PhoneNumber ?? "N/A";

            return new InvoiceDTO
            {
                Id = invoice.Id,
                CustomerId = invoice.CustomerId,
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                TotalAmount = invoice.TotalAmount,
                Date = invoice.Date,
                Subtotal = invoice.Subtotal,
                DiscountPercentage = invoice.DiscountPercentage,
                DiscountAmount = invoice.DiscountAmount,
                GSTPercentage = invoice.GSTPercentage,
                GSTAmount = invoice.GSTAmount,
                Items = invoice.Items?.Select(ii => new InvoiceItemDTO
                {
                    ProductId = ii.ProductId,
                    ProductName = ii.Product?.Name ?? "Unknown Product",
                    Quantity = ii.Quantity,
                    Price = ii.Price
                }).ToList() ?? new List<InvoiceItemDTO>()
            };
        }
    }
}
