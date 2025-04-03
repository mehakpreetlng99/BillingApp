using BillingApp.Data;
using BillingApp.Handlers.Invoices.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

using MediatR;
using BillingApp.Data;
using BillingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace BillingApp.Handlers.Invoices.Handlers
{

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, int>
    {
        private readonly BillingDbContext _context;

        public CreateInvoiceHandler(BillingDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if Customer Exists (Create If Not)
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == request.CustomerPhone, cancellationToken);

            if (customer == null)
            {
                customer = new Customer
                {
                    Name = request.CustomerName,
                    PhoneNumber = request.CustomerPhone
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // 2. Create Invoice
            var invoice = new Invoice
            {
                CustomerId = customer.Id,
                TotalAmount = request.TotalAmount,
                Date = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. Add Invoice Items (Ensure it's not null)
            if (request.Items != null && request.Items.Any())
            {
                foreach (var item in request.Items)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    _context.InvoiceItems.Add(invoiceItem);
                }
                await _context.SaveChangesAsync(cancellationToken);
            }

            // 4. Return Invoice ID
            return invoice.Id;

        }

        //    public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        //    {
        //        // ✅ 1. Check if Customer Exists (Create If Not)
        //        var customer = await _context.Customers
        //            .FirstOrDefaultAsync(c => c.PhoneNumber == request.Invoice.CustomerPhone, cancellationToken);

        //        if (customer == null)
        //        {
        //            customer = new Customer
        //            {
        //                Name = request.Invoice.CustomerName,
        //                PhoneNumber = request.Invoice.CustomerPhone
        //            };
        //            _context.Customers.Add(customer);
        //            await _context.SaveChangesAsync(cancellationToken);
        //        }

        //        // ✅ 2. Create Invoice
        //        var invoice = new Invoice
        //        {
        //            CustomerId = customer.Id,
        //            TotalAmount = request.Invoice.TotalAmount,
        //            Date = DateTime.UtcNow
        //        };

        //        _context.Invoices.Add(invoice);
        //        await _context.SaveChangesAsync(cancellationToken);

        //        // ✅ 3. Add Invoice Items (Ensure it's not null)
        //        if (request.Invoice.Items != null && request.Invoice.Items.Any())
        //        {
        //            foreach (var item in request.Invoice.Items)
        //            {
        //                var invoiceItem = new InvoiceItem
        //                {
        //                    InvoiceId = invoice.Id,
        //                    ProductId = item.ProductId,
        //                    Quantity = item.Quantity,
        //                    Price = item.Price
        //                };
        //                _context.InvoiceItems.Add(invoiceItem);
        //            }

        //            await _context.SaveChangesAsync(cancellationToken);
        //        }

        //        // ✅ 4. Return Invoice ID
        //        return invoice.Id;
        //    }
        //}
    }
}

//    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, int>
//    {
//        private readonly BillingDbContext _context;
//        private readonly IHttpContextAccessor _httpContextAccessor; // Add HttpContextAccessor

//        public CreateInvoiceHandler(BillingDbContext context, IHttpContextAccessor httpContextAccessor)
//        {
//            _context = context;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
//        {
//            // Get AgentId from session
//            var agentId = _httpContextAccessor.HttpContext?.Session.GetString("UserId");

//            if (string.IsNullOrEmpty(agentId))
//            {
//                throw new Exception("Agent ID is missing from session.");
//            }

//            var invoice = new Invoice
//            {
//                CustomerId = request.Invoice.CustomerId,
//                //AgentId = agentId, // Set AgentId from session
//                TotalAmount = request.Invoice.TotalAmount,
//                Date = DateTime.UtcNow,
//                Items = request.Invoice.Items.Select(i => new InvoiceItem
//                {
//                    ProductId = i.ProductId,
//                    Quantity = i.Quantity,
//                    Price = i.Price
//                }).ToList()
//            };

//            _context.Invoices.Add(invoice);
//            await _context.SaveChangesAsync(cancellationToken);

//            return invoice.Id;
//        }
//    }
//}


//using BillingApp.Data;
//using BillingApp.Handlers.Invoices.Commands;
//using BillingApp.Models;
//using MediatR;

//namespace BillingApp.Handlers.Invoices.Handlers
//{
//    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, int>
//    {
//        private readonly BillingDbContext _context;

//        public CreateInvoiceHandler(BillingDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
//        {
//            var invoice = new Invoice
//            {
//                CustomerId = request.Invoice.CustomerId,
//                AgentId = request.Invoice.AgentId,
//                TotalAmount = request.Invoice.TotalAmount,
//                Date = DateTime.UtcNow,
//                Items = request.Invoice.Items.Select(i => new InvoiceItem
//                {
//                    ProductId = i.ProductId,
//                    Quantity = i.Quantity,
//                    Price = i.Price
//                }).ToList()
//            };

//            _context.Invoices.Add(invoice);
//            await _context.SaveChangesAsync(cancellationToken);

//            return invoice.Id;
//        }
//    }
//}
