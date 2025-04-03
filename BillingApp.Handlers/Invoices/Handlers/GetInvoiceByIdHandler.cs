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

        //      public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        //      {
        //          var invoice = await _context.Invoices
        //              .Include(i => i.Customer)  // Explicitly include Customer
        //              .Include(i => i.Items)
        //                  .ThenInclude(ii => ii.Product)  // Include Product for each Item
        //              .AsNoTracking()  // Better for read-only operations
        //              .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        //          if (invoice == null)
        //          {
        //              return null;
        //          }

        //          // Safely handle null Customer
        //          var customerName = invoice.Customer?.Name ?? "Unknown Customer";
        //          var customerPhone = invoice.Customer?.PhoneNumber ?? "N/A";

        //          return new InvoiceDTO
        //          {
        //              Id = invoice.Id,
        //              CustomerId = invoice.CustomerId,
        //              CustomerName = customerName,
        //              CustomerPhone = customerPhone,
        //              TotalAmount = invoice.TotalAmount,
        //              Date = invoice.Date,  // Make sure your DTO has this property
        //              Items = invoice.Items?.Select(ii => new InvoiceItemDTO
        //              {
        //                  ProductId = ii.ProductId,
        //                  ProductName = ii.Product?.Name ?? "Unknown Product",
        //                  Quantity = ii.Quantity,
        //                  Price = ii.Price,
        //// Added calculated field
        //              }).ToList() ?? new List<InvoiceItemDTO>()  // Handle null Items
        //          };
        //      }
        //  }






        //public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        //{
        //        var invoice = await _context.Invoices
        //            .Include(i => i.Items)
        //            .ThenInclude(ii => ii.Product)
        //            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        //        if (invoice == null)
        //        {
        //            return null; // Return null if invoice not found
        //        }

        //        return new InvoiceDTO
        //        {
        //            Id = invoice.Id,
        //            CustomerId = invoice.CustomerId,
        //            CustomerName = invoice.Customer.Name,
        //            CustomerPhone = invoice.Customer.PhoneNumber,
        //            TotalAmount = invoice.TotalAmount,
        //            Items = invoice.Items.Select(ii => new InvoiceItemDTO
        //            {
        //                ProductId = ii.ProductId,
        //                ProductName = ii.Product.Name,
        //                Quantity = ii.Quantity,
        //                Price = ii.Price
        //            }).ToList()
        //        };
        //    }
        //}
        //public class GetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDTO>
        //{
        //    private readonly BillingDbContext _context;

        //    public GetInvoiceByIdHandler(BillingDbContext context)
        //    {
        //        _context = context;
        //    }
        //    public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        //    {
        //        var invoice = await _context.Invoices
        //            .Include(i => i.Customer)  // Ensure Customer details are included
        //            .Include(i => i.Items)
        //                .ThenInclude(ii => ii.Product)  // Ensure Product details are included
        //            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        //        if (invoice == null)
        //        {
        //            Console.WriteLine($"❌ Invoice with ID {request.InvoiceId} not found.");
        //            throw new KeyNotFoundException($"Invoice with ID {request.InvoiceId} not found.");
        //        }

        //        // Debugging each field
        //        Console.WriteLine($"✔️ Invoice Found: ID={invoice.Id}");
        //        Console.WriteLine($"✔️ CustomerId: {invoice.CustomerId}");
        //        Console.WriteLine($"✔️ AgentId: {invoice.AgentId}");
        //        Console.WriteLine($"✔️ TotalAmount: {invoice.TotalAmount}");

        //        if (invoice.Customer == null)
        //        {
        //            Console.WriteLine("❌ Customer object is NULL!");
        //        }
        //        if (invoice.Items == null || !invoice.Items.Any())
        //        {
        //            Console.WriteLine("❌ Invoice Items are NULL or EMPTY!");
        //        }

        //        return new InvoiceDTO
        //        {
        //            Id = invoice.Id,
        //            CustomerId = invoice.CustomerId,
        //            //AgentId = invoice.AgentId.ToString(),  // Convert to string
        //            Date = invoice.Date,
        //            TotalAmount = invoice.TotalAmount,
        //            CustomerPhone = invoice.Customer?.PhoneNumber ?? "N/A", // Ensure safe retrieval
        //            CustomerName = invoice.Customer?.Name ?? "N/A",
        //            Products = invoice.Items?.Select(item => new ProductDTO
        //            {
        //                Id = item.ProductId,
        //                Name = item.Product?.Name ?? "Unknown",  // Ensure safe retrieval
        //                Price = item.Price,
        //                Quantity = item.Quantity
        //            }).ToList() ?? new List<ProductDTO>()  // Ensure list is never null
        //        };
        //    }

        //    public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        //    {
        //        var invoice = await _context.Invoices
        //.Include(i => i.Customer)  // Ensure Customer details are included
        //.Include(i => i.Items)
        //    .ThenInclude(ii => ii.Product)  // Ensure Product details are included
        //.FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        //        if (invoice == null)
        //        {
        //            throw new KeyNotFoundException($"Invoice with ID {request.InvoiceId} not found.");
        //        }

        //        // Log to check if fields are correctly retrieved
        //        Console.WriteLine($"Invoice Found: ID={invoice.Id}, CustomerId={invoice.CustomerId}, AgentId={invoice.AgentId}, TotalAmount={invoice.TotalAmount}");

        //        return new InvoiceDTO
        //        {
        //            Id = invoice.Id,
        //            CustomerId = invoice.CustomerId,
        //            AgentId = invoice.AgentId,
        //            Date = invoice.Date,
        //            TotalAmount = invoice.TotalAmount,
        //            CustomerPhone = invoice.Customer?.PhoneNumber ?? "N/A", // Ensure safe retrieval
        //            CustomerName = invoice.Customer?.Name ?? "N/A",
        //            Products = invoice.Items.Select(item => new ProductDTO
        //            {
        //                Id = item.ProductId,
        //                Name = item.Product?.Name ?? "Unknown",  // Ensure safe retrieval
        //                Price = item.Price,
        //                Quantity = item.Quantity
        //            }).ToList()
        //        };

        //var invoice = await _context.Invoices
        //    .Include(i => i.Items)
        //    .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        //if (invoice == null)
        //{
        //    throw new KeyNotFoundException($"Invoice with ID {request.InvoiceId} not found.");
        //}

        //return new InvoiceDTO
        //{
        //    Id = invoice.Id,
        //    CustomerId = invoice.CustomerId,

        //    AgentId = invoice.AgentId,
        //    Date = invoice.Date,
        //    TotalAmount = invoice.TotalAmount,
        //    Products = invoice.Items.Select(item => new ProductDTO
        //    {
        //        Id = item.ProductId,
        //        Name = item.Product.Name, // Ensure Product navigation is loaded
        //        Price = item.Price,
        //        Quantity = item.Quantity
        //    }).ToList()
        //};
    }

    //public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    //{
    //    var invoice = await _context.Invoices
    //        .Include(i => i.Items)
    //        .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

    //    if (invoice == null) return null;
    //    return new InvoiceDTO
    //    {
    //        Id = invoice.Id,
    //        CustomerId = invoice.CustomerId,
    //        CustomerName = invoice.Customer.Name, // ✅ Fetch Name from Customer
    //        CustomerPhone = invoice.Customer.PhoneNumber, // ✅ Fetch Phone from Customer
    //        AgentId = invoice.AgentId,
    //        TotalAmount = invoice.TotalAmount,
    //        Date = invoice.Date,
    //        Items = invoice.Items.Select(item => new InvoiceItemDTO
    //        {
    //            ProductId = item.ProductId,
    //            Quantity = item.Quantity,
    //            Price = item.Price
    //        }).ToList()
    //    };

    //return new InvoiceDTO
    //{
    //    Id = invoice.Id,
    //    CustomerId = invoice.CustomerId,
    //    AgentId = invoice.AgentId,
    //    TotalAmount = invoice.TotalAmount,
    //    Date = invoice.Date,
    //    Items = invoice.Items.Select(i => new InvoiceItemDTO
    //    {
    //        ProductId = i.ProductId,
    //        Quantity = i.Quantity,
    //        Price = i.Price
    //    }).ToList()
    //};

