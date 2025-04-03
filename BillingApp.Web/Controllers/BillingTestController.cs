using BillingApp.DTO;
using BillingApp.Handlers.Invoices.Commands;
using BillingApp.Handlers.Invoices.Queries;
using BillingApp.Handlers.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Web.Controllers
{
    public class BillingTestController : Controller
    {
        private readonly IMediator _mediator;

        public BillingTestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> CreateInvoice()
        {
            var products = await _mediator.Send(new GetAllProductsQuery()); // Fetch products from database
            ViewBag.Products = products; // Pass products to the view
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromForm] CreateInvoiceCommand command)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                // Create a new InvoiceDTO to return to the view
                var invoiceDto = new InvoiceDTO
                {
                    CustomerPhone = command.CustomerPhone,
                    CustomerName = command.CustomerName,
                    Items = command.Items,
                    TotalAmount = command.TotalAmount
                };

                return View(invoiceDto); // Return the form with validation errors
            }

            // Call CreateInvoiceHandler via Mediator
            int invoiceId = await _mediator.Send(command);

            // Redirect to Invoice View
            return RedirectToAction("InvoiceDetails", new { id = invoiceId });
        }
        
       

        [HttpGet]
        public async Task<IActionResult> InvoiceDetails(int id)
        {
            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
    }

}