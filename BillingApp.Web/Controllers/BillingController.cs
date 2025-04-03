//using Microsoft.AspNetCore.Mvc;
//using MediatR;
//using BillingApp.DTO;
//using BillingApp.Handlers.Invoices.Commands;
//using BillingApp.Handlers.Invoices.Queries;
//using BillingApp.Handlers.Customers.Queries;
//using BillingApp.Handlers.Customers.Commands;
////using BillingApp.Web.Services;
//using System.Threading.Tasks;
//using BillingApp.Data;
//using BillingApp.Models;
//using BillingApp.Handlers.Products.Queries;
//using Microsoft.AspNetCore.Http;


//namespace BillingApp.Web.Controllers
//{
//    public class BillingController : Controller
//    {
//        private readonly IMediator _mediator;
//        private readonly BillingDbContext _context;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        //private readonly PdfService _pdfService;
//        //private readonly ViewRenderService _viewRenderService; // Added ViewRenderService

//        public BillingController(IMediator mediator, BillingDbContext context, IHttpContextAccessor httpContextAccessor)
//        {
//            _mediator = mediator;
//            _context = context;
//            _httpContextAccessor = httpContextAccessor;
//            //_pdfService = pdfService;
//            //_viewRenderService = viewRenderService;
//        }

//        //public IActionResult Create()
//        //{
//        //    return View();
//        //}
//        //public IActionResult Create()
//        //{
//        //    var model = new InvoiceDTO
//        //    {
//        //        Products = new List<ProductDTO>() // Ensure it's initialized
//        //    };

//        //    return View(model);
//        //}

//        //public IActionResult Create()
//        //{
//        //    ViewBag.Products = _context.Products
//        //        .Select(p => new ProductDTO { Id = p.Id, Name = p.Name, Price = p.Price })
//        //        .ToList();

//        //    return View(new InvoiceDTO());
//        //}
//        public IActionResult Create()
//        {
//            var products = _context.Products
//                .Select(p => new ProductDTO { Id = p.Id, Name = p.Name, Price = p.Price })
//                .ToList();

//            ViewBag.Products = products ?? new List<ProductDTO>();

//            return View(new InvoiceDTO());
//        }



//        [HttpPost]
//        public async Task<IActionResult> Create(InvoiceDTO model)
//        {
//            if (!ModelState.IsValid)
//            {
//                foreach (var key in ModelState.Keys)
//                {
//                    foreach (var error in ModelState[key].Errors)
//                    {
//                        Console.WriteLine($"❌ Validation error in {key}: {error.ErrorMessage}");
//                    }
//                }
//                return View(model);
//            }

//            // Retrieve AgentId from session
//            //var agentId = HttpContext.Session.GetString("UserId");
//            var agentId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

//            Console.WriteLine($"🔹 Retrieved AgentId from session: {agentId}");

//            if (string.IsNullOrEmpty(agentId))
//            {
//                Console.WriteLine("❌ Error: AgentId is missing from the session.");
//                ModelState.AddModelError("AgentId", "AgentId is required.");
//                return View(model);
//            }

//            var customer = await _mediator.Send(new GetCustomerByPhoneQuery(model.CustomerPhone));
//            if (customer == null)
//            {
//                var customerId = await _mediator.Send(new AddCustomerCommand(new CustomerDTO
//                {
//                    Name = model.CustomerName,
//                    PhoneNumber = model.CustomerPhone
//                }));

//                model.CustomerId = customerId;
//            }
//            else
//            {
//                model.CustomerId = customer.Id;
//                model.CustomerName = customer.Name;
//            }

//            Console.WriteLine($"✅ Creating Invoice with AgentId: {agentId}");

//            var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//            {
//                CustomerId = model.CustomerId,
//                //AgentId = agentId,  // ✅ Using AgentId from session
//                TotalAmount = model.TotalAmount,
//                Items = model.Items
//            }));

//            return RedirectToAction("Invoice", new { id = invoiceId });
//        }

//        //public async Task<IActionResult> Create(InvoiceDTO model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        foreach (var key in ModelState.Keys)
//        //        {
//        //            foreach (var error in ModelState[key].Errors)
//        //            {
//        //                Console.WriteLine($"Validation error in {key}: {error.ErrorMessage}");
//        //            }
//        //        }
//        //        return View(model);
//        //    }

//        //    // Retrieve AgentId from session
//        //    var agentId = HttpContext.Session.GetString("UserId");

//        //    if (string.IsNullOrEmpty(agentId))
//        //    {
//        //        Console.WriteLine("Error: AgentId is missing from the session.");
//        //        ModelState.AddModelError("AgentId", "AgentId is required.");
//        //        return View(model);
//        //    }

//        //    var customer = await _mediator.Send(new GetCustomerByPhoneQuery(model.CustomerPhone));
//        //    if (customer == null)
//        //    {
//        //        var customerId = await _mediator.Send(new AddCustomerCommand(new CustomerDTO
//        //        {
//        //            Name = model.CustomerName,
//        //            PhoneNumber = model.CustomerPhone
//        //        }));

//        //        model.CustomerId = customerId;
//        //    }
//        //    else
//        //    {
//        //        model.CustomerId = customer.Id;
//        //        model.CustomerName = customer.Name;
//        //    }

//        //    var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//        //    {
//        //        CustomerId = model.CustomerId,
//        //        AgentId = agentId,  // Assign AgentId from session
//        //        TotalAmount = model.TotalAmount,
//        //        Items = model.Items
//        //    }));

//        //    return RedirectToAction("Invoice", new { id = invoiceId });
//        //}

//        //public async Task<IActionResult> Create(InvoiceDTO model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        foreach (var key in ModelState.Keys)
//        //        {
//        //            foreach (var error in ModelState[key].Errors)
//        //            {
//        //                Console.WriteLine($"Validation error in {key}: {error.ErrorMessage}");
//        //            }
//        //        }
//        //        return View(model);
//        //    }

//        //    var customer = await _mediator.Send(new GetCustomerByPhoneQuery(model.CustomerPhone));
//        //    if (customer == null)
//        //    {
//        //        var customerId = await _mediator.Send(new AddCustomerCommand(new CustomerDTO
//        //        {
//        //            Name = model.CustomerName,
//        //            PhoneNumber = model.CustomerPhone
//        //        }));

//        //        model.CustomerId = customerId;
//        //    }
//        //    else
//        //    {
//        //        model.CustomerId = customer.Id;
//        //        model.CustomerName = customer.Name;
//        //    }

//        //    var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//        //    {
//        //        CustomerId = model.CustomerId,
//        //        AgentId = model.AgentId,
//        //        TotalAmount = model.TotalAmount,
//        //        Items = model.Items
//        //    }));

//        //    return RedirectToAction("Invoice", new { id = invoiceId });
//        //}


//        [HttpPost]
//        public async Task<IActionResult> GenerateInvoice(InvoiceDTO model)
//        {
//            if (!ModelState.IsValid)
//            {
//                model.Products = await _mediator.Send(new GetAllProductsQuery());
//                return View("Create", model);
//            }

//            // Get the AgentId from Session
//            //var agentId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored as a string
//            //var agentIdString = HttpContext.Session.GetString("AgentId");
//            var agentId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

//            //Console.WriteLine($"Agent ID from session: {agentIdString}");

//            if (string.IsNullOrEmpty(agentId))
//            {
//                ModelState.AddModelError("AgentId", "Agent ID is missing from the session.");
//                model.Products = await _mediator.Send(new GetAllProductsQuery());
//                return View("Create", model);
//            }

//            // Use the retrieved AgentId
//            var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//            {
//                CustomerId = model.CustomerId,
//                CustomerName = model.CustomerName,
//                CustomerPhone = model.CustomerPhone,
//                //AgentId = agentId,  // ✅ Ensure it's assigned properly
//                TotalAmount = model.TotalAmount,
//                Items = model.Items
//            }));

//            return RedirectToAction("Invoice", new { id = invoiceId });
//        }

//        //public async Task<IActionResult> GenerateInvoice(InvoiceDTO model)
//        //{
//        //    // Retrieve AgentId from session
//        //    var agentIdString = HttpContext.Session.GetString("AgentId");

//        //    Console.WriteLine($"Agent ID from session: {agentIdString}");

//        //    if (string.IsNullOrEmpty(agentIdString))
//        //    {
//        //        ModelState.AddModelError("AgentId", "Agent ID is missing. Please log in again.");
//        //    }
//        //    else
//        //    {
//        //        model.AgentId = agentIdString; // Ensure it's assigned to the model
//        //    }

//        //    if (!ModelState.IsValid)
//        //    {
//        //        // Log validation errors
//        //        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
//        //        {
//        //            Console.WriteLine(error.ErrorMessage);
//        //        }

//        //        // Reload products for dropdown
//        //        model.Products = await _mediator.Send(new GetAllProductsQuery());
//        //        return View("Create", model);
//        //    }

//        //    // Create Invoice using Mediator
//        //    var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//        //    {
//        //        CustomerId = model.CustomerId,
//        //        CustomerName = model.CustomerName,
//        //        CustomerPhone = model.CustomerPhone,
//        //        AgentId = model.AgentId, // Now correctly set from session
//        //        TotalAmount = model.TotalAmount,
//        //        Items = model.Items
//        //    }));

//        //    return RedirectToAction("InvoiceDetails", new { id = invoiceId });
//        //}

//        //public async Task<IActionResult> GenerateInvoice(InvoiceDTO model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        // Log validation errors to check what's going wrong
//        //        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
//        //        {
//        //            Console.WriteLine(error.ErrorMessage);
//        //        }

//        //        // Reload products for dropdown
//        //        model.Products = await _mediator.Send(new GetAllProductsQuery());
//        //        return View("Create", model);
//        //    }

//        //    // Fetch AgentId from session
//        //    var agentIdString = HttpContext.Session.GetString("AgentId");
//        //    if (string.IsNullOrEmpty(agentIdString))
//        //    {
//        //        // Handle case where AgentId is not set in session
//        //        ModelState.AddModelError("", "Agent information is missing. Please log in again.");
//        //        model.Products = await _mediator.Send(new GetAllProductsQuery());
//        //        return View("Create", model);
//        //    }

//        //    if (!int.TryParse(agentIdString, out int agentId))
//        //    {
//        //        ModelState.AddModelError("", "Invalid Agent ID. Please log in again.");
//        //        model.Products = await _mediator.Send(new GetAllProductsQuery());
//        //        return View("Create", model);
//        //    }

//        //    // Create Invoice using Mediator
//        //    var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//        //    {
//        //        CustomerId = model.CustomerId,
//        //        CustomerName = model.CustomerName,
//        //        CustomerPhone = model.CustomerPhone,
//        //        AgentId = agentId.ToString(), // ✅ Fetching from session instead of model
//        //        TotalAmount = model.TotalAmount,
//        //        Items = model.Items
//        //    }));

//        //    return RedirectToAction("InvoiceDetails", new { id = invoiceId });
//        //}

//        //public async Task<IActionResult> GenerateInvoice(InvoiceDTO model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        // Log validation errors to check what's going wrong
//        //        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
//        //        {
//        //            Console.WriteLine(error.ErrorMessage);
//        //        }

//        //        // Reload products for dropdown
//        //        model.Products = await _mediator.Send(new GetAllProductsQuery());
//        //        return View("Create", model);
//        //    }

//        //    // Create Invoice using Mediator
//        //    var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
//        //    {
//        //        CustomerId = model.CustomerId,
//        //        CustomerName=model.CustomerName,
//        //        CustomerPhone=model.CustomerPhone,
//        //        AgentId = model.AgentId,
//        //        TotalAmount = model.TotalAmount,
//        //        Items = model.Items
//        //    }));

//        //    return RedirectToAction("InvoiceDetails", new { id = invoiceId });
//        //}
//        //public IActionResult GenerateInvoice(InvoiceDTO model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        // Log validation errors to check what's going wrong
//        //        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
//        //        {
//        //            Console.WriteLine(error.ErrorMessage);
//        //        }

//        //        // Reload products for dropdown
//        //        model.Products = _context.Products.Select(p => new ProductDTO
//        //        {
//        //            Id = p.Id,
//        //            Name = p.Name,
//        //            Price = p.Price
//        //        }).ToList();

//        //        return View("Create", model);
//        //    }

//        //    // Save invoice (example)
//        //    var invoice = new Invoice
//        //    {
//        //        CustomerId = model.CustomerId,
//        //        AgentId = model.AgentId,
//        //        Date = DateTime.Now, // ✅ Use Date instead of CreatedAt
//        //        TotalAmount = model.TotalAmount,
//        //        Items = model.Products.Select(p => new InvoiceItem
//        //        {
//        //            ProductId = p.Id,   // Make sure ProductId exists in your DTO
//        //            Quantity = p.Quantity,
//        //            Price = p.Price
//        //        }).ToList()
//        //    };

//        //    _context.Invoices.Add(invoice);
//        //    _context.SaveChanges();

//        //    return RedirectToAction("InvoiceDetails", new { id = invoice.Id });
//        //}

//        public async Task<IActionResult> Invoice(int id)
//        {
//            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
//            if (invoice == null)
//            {
//                return NotFound();
//            }

//            return View(invoice);
//        }

//        //public async Task<IActionResult> DownloadPDF(int id)
//        //{
//        //    var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
//        //    if (invoice == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    //string invoiceHtml = await _viewRenderService.RenderViewToStringAsync(this, "Invoice", invoice);
//        //    byte[] pdfBytes = _pdfService.GeneratePdfFromHtml(invoiceHtml);

//        //    return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Id}.pdf");
//        //}
//    }
//}


////using Microsoft.AspNetCore.Mvc;
////using MediatR;
////using BillingApp.DTO;
////using BillingApp.Handlers.Invoices.Commands;
////using BillingApp.Handlers.Invoices.Queries;
////using BillingApp.Handlers.Customers.Queries;
////using BillingApp.Handlers.Customers.Commands;
////using BillingApp.Web.Services;


////namespace BillingApp.Web.Controllers
////{
////    public class BillingController : Controller
////    {
////        private readonly IMediator _mediator;
////        private readonly PdfService _pdfService;

////        public BillingController(IMediator mediator, PdfService pdfService)
////        {
////            _mediator = mediator;
////            _pdfService = pdfService;
////        }

////        public IActionResult Create()
////        {
////            return View();
////        }

////        [HttpPost]
////        public async Task<IActionResult> Create(InvoiceDTO model)
////        {
////            if (!ModelState.IsValid)
////            {
////                return View(model);
////            }

////            // Check if the customer exists
////            var customer = await _mediator.Send(new GetCustomerByPhoneQuery(model.CustomerPhone));
////            if (customer == null)
////            {
////                // If customer doesn't exist, create a new one
////                var customerId = await _mediator.Send(new AddCustomerCommand(new CustomerDTO
////                {
////                    Name = model.CustomerName,
////                    PhoneNumber = model.CustomerPhone
////                }));

////                model.CustomerId = customerId;
////            }
////            else
////            {
////                model.CustomerId = customer.Id;
////                model.CustomerName = customer.Name; // Fetching name from DB
////            }

////            // Create Invoice
////            var invoiceId = await _mediator.Send(new CreateInvoiceCommand(new InvoiceDTO
////            {
////                CustomerId = model.CustomerId,
////                AgentId = model.AgentId, // The logged-in agent's ID
////                TotalAmount = model.TotalAmount,
////                Items = model.Items
////            }));

////            return RedirectToAction("Invoice", new { id = invoiceId });
////        }

////        public async Task<IActionResult> Invoice(int id)
////        {
////            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
////            if (invoice == null)
////            {
////                return NotFound();
////            }

////            return View(invoice);
////        }

////        public async Task<IActionResult> DownloadPDF(int id)
////        {
////            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
////            if (invoice == null)
////            {
////                return NotFound();
////            }

////            var renderer = new HtmlToPdfConverter();
////            var pdf = renderer.GeneratePdf(await this.RenderViewToStringAsync("Invoice", invoice));

////            return File(pdf, "application/pdf", $"Invoice_{invoice.Id}.pdf");
////        }

////    }
////}
