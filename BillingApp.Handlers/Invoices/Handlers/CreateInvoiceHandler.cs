

using BillingApp.Data;
using BillingApp.DTO;
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
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
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

                // 2. Verify products and update inventory before creating invoice
                var productUpdates = new List<Product>();
                foreach (var item in request.Items ?? Enumerable.Empty<InvoiceItemDTO>())
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId, cancellationToken);

                    if (product == null)
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found");
                    }

                    if (product.IsActive == false)
                    {
                        throw new Exception($"Product {product.Name} is inactive");
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        throw new Exception($"Insufficient quantity for {product.Name}. Available: {product.Quantity}");
                    }

                    // Update inventory
                    product.Quantity -= item.Quantity;
                    product.IsActive = product.Quantity > 0;
                    productUpdates.Add(product);
                }

                // 3. Create Invoice
                var invoice = new Invoice
                {
                    CustomerId = customer.Id,
                    Date = DateTime.UtcNow
                };

                // 4. Calculate Subtotal
                invoice.Subtotal = request.Items?.Sum(item => item.Quantity * item.Price) ?? 0;

                // 5. Apply Discount
                invoice.DiscountPercentage = request.DiscountPercentage ?? 5;
                invoice.DiscountAmount = invoice.Subtotal * (invoice.DiscountPercentage / 100);

                // 6. Apply GST
                invoice.GSTPercentage = request.GSTPercentage ?? 10;
                var amountAfterDiscount = invoice.Subtotal - (invoice.DiscountAmount ?? 0);
                invoice.GSTAmount = amountAfterDiscount * (invoice.GSTPercentage / 100);

                // 7. Calculate Total
                invoice.TotalAmount = amountAfterDiscount + (invoice.GSTAmount ?? 0);

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync(cancellationToken);

                // 8. Add Invoice Items
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
                }

                // Save all changes (invoice items and product updates)
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return invoice.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Invoice creation failed: " + ex.Message, ex);
            }
        }

    }
}



