using BillingApp.DTO;
using BillingApp.Handlers.Users.Commands;
using BillingApp.Handlers.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Web.Controllers
{

    // BillingApp.Web.Controllers/SuperAdminController.cs

    [Authorize(Policy = "RequireSuperAdminRole")]
    public class SuperAdminController : Controller
    {
        private readonly IMediator _mediator;

        public SuperAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new RegisterUserCommand
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role
                });

                if (result)
                    return RedirectToAction("ManageUsers", "SuperAdmin");

                ModelState.AddModelError("", "User creation failed.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand(userId));

            if (result)
                return RedirectToAction("ManageUsers", "SuperAdmin");

            ModelState.AddModelError("", "User deletion failed.");
            return RedirectToAction("ManageUsers", "SuperAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new UpdateUserCommand
                {
                    UserId = model.Id,
                    FullName = model.FullName,
                    Email = model.Email,
                    Role = model.Role
                });

                if (result)
                    return RedirectToAction("ManageUsers", "SuperAdmin");

                ModelState.AddModelError("", "User update failed.");
            }

            return View(model);
        }

        // New action for fetching user by ID
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ManageUsers", "SuperAdmin");
            }

            return View(user);
        }

        public IActionResult Index()
        {
            return View();
        }
    }


}
