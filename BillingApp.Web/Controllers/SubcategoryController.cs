using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Categories.Queries;
using BillingApp.Handlers.Subcategories.Commands;
using BillingApp.Handlers.Subcategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Web.Controllers
{
    public class SubcategoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SubcategoryController> _logger;
        private readonly BillingDbContext _context;

        public SubcategoryController(IMediator mediator, ILogger<SubcategoryController> logger, BillingDbContext context)
        {
            _mediator = mediator;
            _logger = logger;
            _context=context;
        }

        public async Task<IActionResult> Index()
        {
            var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
            var categories = await _mediator.Send(new GetCategoriesQuery());

         
            ViewBag.CategoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

            return View(subcategories);
        }

  
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            ViewBag.Categories = new SelectList(categories, "Id", "Name"); 

            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create(SubcategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
            
                var categories = await _mediator.Send(new GetCategoriesQuery());
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            
            var result = await _mediator.Send(new AddSubcategoryCommand (model));

            if (result)
            {
                TempData["AlertMessage"] = "SubCategory added successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Subcategory with Name {model.Name} created successfully.");
                return RedirectToAction("Index");
            }

            _logger.LogWarning($"Failed to create subcategory with Name {model.Name}.");
            return RedirectToAction("Index");
        }

   
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var subcategory = await _mediator.Send(new GetSubcategoryByIdQuery { Id = id });
            if (subcategory == null)
            {
                return NotFound();
            }

            var categories = await _mediator.Send(new GetCategoriesQuery());
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(subcategory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubcategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _mediator.Send(new GetCategoriesQuery());
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            var result = await _mediator.Send(new UpdateSubcategoryCommand { Subcategory = model });

            if (result)
            {
                TempData["AlertMessage"] = "SubCategory Edited successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Subcategory with ID {model.Id} updated successfully.");
                return RedirectToAction("Index");
            }

            _logger.LogWarning($"Failed to update subcategory with ID {model.Id}.");
            return RedirectToAction("Index");
        }


      

       
        [HttpGet]
    
        public async Task<IActionResult> Delete(int id)
        {
            var subcategory = await _context.Subcategories
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    CategoryName = _context.Categories
                        .Where(c => c.Id == s.CategoryId)
                        .Select(c => c.Name)
                        .FirstOrDefault() 
                })
                .FirstOrDefaultAsync(); 

            if (subcategory == null)
            {
                return NotFound();
            }

          
            var subcategoryDTO = new SubcategoryDTO
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                CategoryName = subcategory.CategoryName
            };

            return View(subcategoryDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SubcategoryDTO model)
        {
            var result = await _mediator.Send(new DeleteSubcategoryCommand { Id = model.Id });

            if (result)
            {
                TempData["AlertMessage"] = "SubCategory deleted successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Subcategory with ID {model.Id} deleted successfully.");
                return RedirectToAction("Index");
            }

            _logger.LogWarning($"Failed to delete subcategory with ID {model.Id}.");
            return RedirectToAction("Index");
        }

       
    }
}

