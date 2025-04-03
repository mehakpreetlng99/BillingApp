

using BillingApp.Data;
using BillingApp.Handlers.Invoices.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                Date = DateTime.UtcNow
            };

            // 3. Calculate Subtotal
            invoice.Subtotal = request.Items?.Sum(item => item.Quantity * item.Price) ?? 0;

            // 4. Apply Discount (default to 5% if not provided)
            invoice.DiscountPercentage = request.DiscountPercentage ?? 5; // Default to 5%
            invoice.DiscountAmount = invoice.Subtotal * (invoice.DiscountPercentage / 100);

            // 5. Apply GST (default to 10% if not provided)
            invoice.GSTPercentage = request.GSTPercentage ?? 10; // Default to 10%
            var amountAfterDiscount = invoice.Subtotal - (invoice.DiscountAmount ?? 0);
            invoice.GSTAmount = amountAfterDiscount * (invoice.GSTPercentage / 100);

            // 6. Calculate Total
            invoice.TotalAmount = amountAfterDiscount + (invoice.GSTAmount ?? 0);

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync(cancellationToken);

            // 7. Add Invoice Items (Ensure it's not null)
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

            // 8. Return Invoice ID
            return invoice.Id;
        }
 



//using BillingApp.Data;
//using BillingApp.Handlers.Invoices.Commands;
//using BillingApp.Models;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
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
//            // 1. Check if Customer Exists (Create If Not)
//            var customer = await _context.Customers
//                .FirstOrDefaultAsync(c => c.PhoneNumber == request.CustomerPhone, cancellationToken);

//            if (customer == null)
//            {
//                customer = new Customer
//                {
//                    Name = request.CustomerName,
//                    PhoneNumber = request.CustomerPhone
//                };
//                _context.Customers.Add(customer);
//                await _context.SaveChangesAsync(cancellationToken);
//            }

//            // 2. Create Invoice
//            var invoice = new Invoice
//            {
//                CustomerId = customer.Id,
//                TotalAmount = request.TotalAmount,
//                Date = DateTime.UtcNow
//            };

//            _context.Invoices.Add(invoice);
//            await _context.SaveChangesAsync(cancellationToken);

//            // 3. Add Invoice Items (Ensure it's not null)
//            if (request.Items != null && request.Items.Any())
//            {
//                foreach (var item in request.Items)
//                {
//                    var invoiceItem = new InvoiceItem
//                    {
//                        InvoiceId = invoice.Id,
//                        ProductId = item.ProductId,
//                        Quantity = item.Quantity,
//                        Price = item.Price
//                    };
//                    _context.InvoiceItems.Add(invoiceItem);
//                }
//                await _context.SaveChangesAsync(cancellationToken);
//            }

//            // 4. Return Invoice ID
//            return invoice.Id;

//        }








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
