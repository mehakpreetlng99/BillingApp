using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using BillingApp.DTO;
using BillingApp.Handlers.Products.Commands;
using BillingApp.Handlers.Products.Queries;
using System.Collections.Generic;
using BillingApp.Handlers.Subcategories.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BillingApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
            ViewBag.Subcategories = new SelectList(subcategories, "Id", "Name");
            return View();
        }


        [HttpPost]
    
        public async Task<IActionResult> Create(ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
                ViewBag.Subcategories = new SelectList(subcategories, "Id", "Name");
                return View(model);
            }

            var result = await _mediator.Send(new AddProductCommand(model));
            if (result)
            {
                TempData["AlertMessage"] = "Product Created successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Product {model.Name} created successfully.");
                return RedirectToAction("Index");
            }
            _logger.LogWarning($"Failed to create product {model.Name}.");
            return View(model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }
           
            var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
            ViewBag.Subcategories = new SelectList(subcategories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
                ViewBag.Subcategories = new SelectList(subcategories, "Id", "Name");
                return View(model);
            }

            var result = await _mediator.Send(new UpdateProductCommand(model));
            if (result)
            {
                TempData["AlertMessage"] = "Product updated successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Product {model.Name} updated successfully.");
                return RedirectToAction("Index");
            }
            _logger.LogWarning($"Failed to update product {model.Name}.");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            if (result)
            {
                TempData["AlertMessage"] = "Product deleted successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
                return RedirectToAction("Index");
            }
            _logger.LogWarning($"Failed to delete product with ID {id}.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }
            return View(product);
        }
    }
}
