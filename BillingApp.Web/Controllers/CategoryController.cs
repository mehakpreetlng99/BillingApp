using BillingApp.DTO;
using BillingApp.Handlers.Categories.Commands;
using BillingApp.Handlers.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

       
        public IActionResult CreateCategory()
        {
            return View(new CategoryDTO());
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new CreateCategoryCommand { Name = model.Name });

                if (result)
                {
                    TempData["AlertMessage"] = "Category Added Sucessfully!";
                    TempData["AlertType"] = "success";
                    _logger.LogInformation("Category created successfully.");
                    return RedirectToAction("GetCategories"); 
                }

                ModelState.AddModelError("", "Category creation failed.");
            }

            return View(model);
        }

       
        public IActionResult EditCategory(int id)
        {
            return View(new CategoryDTO { Id = id });
        }

        
        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new UpdateCategoryCommand { Id = model.Id, Name = model.Name });

                if (result)
                {
                    TempData["AlertMessage"] = "Category edited successfully!";
                    TempData["AlertType"] = "success";
                    _logger.LogInformation($"Category with ID {model.Id} updated successfully.");
                    return RedirectToAction("GetCategories");
                }

                ModelState.AddModelError("", "Category update failed.");
            }

            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand { Id = id });

            if (result)
            {
                TempData["AlertMessage"] = "Category Deleted successfully!";
                TempData["AlertType"] = "success";
                _logger.LogInformation($"Category with ID {id} deleted successfully.");
                return RedirectToAction("GetCategories");
            }

            _logger.LogWarning($"Failed to delete category with ID {id}.");
            return RedirectToAction("GetCategories");
        }
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (category == null)
            {
                
                return NotFound();
            }

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryDTO); 
        }

        
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());

            if (categories.Count == 0)
            {
             
                return View("NoCategoriesFound");
            }

            var categoryDTOs = categories.ConvertAll(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            });

            return View(categoryDTOs); 
        }
        public IActionResult NoCategoriesFound()
        {
            return View();
        }
    }
}
