using BillingApp.DTO;
using BillingApp.Handlers.Categories.Queries;
using BillingApp.Handlers.Subcategories.Commands;
using BillingApp.Handlers.Subcategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BillingApp.Web.Controllers
{
    public class SubcategoryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SubcategoryController> _logger;

        public SubcategoryController(IMediator mediator, ILogger<SubcategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /Subcategory
        //public async Task<IActionResult> Index()
        //{
        //    var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
        //    return View(subcategories);
        //}
        public async Task<IActionResult> Index()
        {
            var subcategories = await _mediator.Send(new GetSubcategoriesQuery());
            var categories = await _mediator.Send(new GetCategoriesQuery());

            // Store Category Names in ViewBag as Dictionary
            ViewBag.CategoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

            return View(subcategories);
        }

        // GET: /Subcategory/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            ViewBag.Categories = new SelectList(categories, "Id", "Name"); // Populate dropdown

            return View();
        }

        // POST: /Subcategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(SubcategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                // If model is invalid, return to the view with the existing categories
                var categories = await _mediator.Send(new GetCategoriesQuery());
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            // Your create logic
            var result = await _mediator.Send(new AddSubcategoryCommand (model));

            if (result)
            {
                _logger.LogInformation($"Subcategory with Name {model.Name} created successfully.");
                return RedirectToAction("Index");
            }

            _logger.LogWarning($"Failed to create subcategory with Name {model.Name}.");
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Create(SubcategoryDTO subcategoryDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(subcategoryDto);
        //    }
        //    var result = await _mediator.Send(new AddSubcategoryCommand(subcategoryDto));


        //    if (result)
        //    {
        //        _logger.LogInformation($"Subcategory '{subcategoryDto.Name}' created successfully.");
        //        return RedirectToAction(nameof(Index));
        //    }

        //    _logger.LogWarning($"Failed to create subcategory '{subcategoryDto.Name}'.");
        //    ModelState.AddModelError("", "Failed to create subcategory.");
        //    return View(subcategoryDto);
        //}
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
                _logger.LogInformation($"Subcategory with ID {model.Id} updated successfully.");
                return RedirectToAction("Index");
            }

            _logger.LogWarning($"Failed to update subcategory with ID {model.Id}.");
            return RedirectToAction("Index");
        }


        //// GET: /Subcategory/Edit/{id}
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var subcategory = await _mediator.Send(new GetSubcategoryByIdQuery { Id = id });

        //    if (subcategory == null)
        //    {
        //        _logger.LogWarning($"Subcategory with ID {id} not found.");
        //        return NotFound();
        //    }

        //    return View(subcategory);
        //}

        //// POST: /Subcategory/Edit/{id}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(SubcategoryDTO subcategoryDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(subcategoryDto);
        //    }

        //    var result = await _mediator.Send(new UpdateSubcategoryCommand { Subcategory = subcategoryDto });

        //    if (result)
        //    {
        //        _logger.LogInformation($"Subcategory '{subcategoryDto.Name}' updated successfully.");
        //        return RedirectToAction(nameof(Index));
        //    }

        //    _logger.LogWarning($"Failed to update subcategory '{subcategoryDto.Name}'.");
        //    ModelState.AddModelError("", "Failed to update subcategory.");
        //    return View(subcategoryDto);
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var subcategory = await _mediator.Send(new GetSubcategoryByIdQuery { Id = id });
            if (subcategory == null)
            {
                return NotFound();
            }

            return View(subcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SubcategoryDTO model)
        {
            var result = await _mediator.Send(new DeleteSubcategoryCommand { Id = model.Id });

            if (result)
            {
                _logger.LogInformation($"Subcategory with ID {model.Id} deleted successfully.");
                return RedirectToAction("Index");
            }

            _logger.LogWarning($"Failed to delete subcategory with ID {model.Id}.");
            return RedirectToAction("Index");
        }

        // GET: /Subcategory/Delete/{id}
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var subcategory = await _mediator.Send(new GetSubcategoryByIdQuery { Id = id });

        //    if (subcategory == null)
        //    {
        //        _logger.LogWarning($"Subcategory with ID {id} not found.");
        //        return NotFound();
        //    }

        //    return View(subcategory);
        //}

        //// POST: /Subcategory/Delete/{id}
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var result = await _mediator.Send(new DeleteSubcategoryCommand { Id = id });

        //    if (result)
        //    {
        //        _logger.LogInformation($"Subcategory with ID {id} deleted successfully.");
        //        return RedirectToAction(nameof(Index));
        //    }

        //    _logger.LogWarning($"Failed to delete subcategory with ID {id}.");
        //    return RedirectToAction(nameof(Index));
        //}
    }
}

