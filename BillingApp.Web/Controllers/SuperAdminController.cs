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

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View(new UserDTO());
        }
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);

            
        }
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return View(users);
        }

        [HttpPost]

        public async Task<IActionResult> RegisterUser(UserDTO model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new RegisterUserCommand
                {
                    
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = string.IsNullOrEmpty(model.Role) ? "Agent" : model.Role // Default to "Agent" if Role is empty
                });

                if (result)
                {
                    return RedirectToAction("ManageUsers", "SuperAdmin");
                }
                else
                {
                    ModelState.AddModelError("", "User already exists or invalid data.");
                }
            }
            else
            {
                // Log validation errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            return View(model);
        }

        //public async Task<IActionResult> RegisterUser(UserDTO model)
        //{
        //    if (model == null)
        //    {
        //        // Log or handle the null model case
        //        return BadRequest("Model cannot be null.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _mediator.Send(new RegisterUserCommand
        //        {
        //            FullName = model.FullName,
        //            Email = model.Email,
        //            Password = model.Password,
        //            Role = model.Role // Assuming role is part of the user registration
        //        });

        //        if (result)
        //        {
        //            return RedirectToAction("ManageUsers", "SuperAdmin"); // Redirect to user management page after success
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "User already exists or invalid data.");
        //        }
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> RegisterUser(UserDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _mediator.Send(new RegisterUserCommand
        //        {
        //            FullName = model.FullName,
        //            Email = model.Email,
        //            Password = model.Password,
        //            Role = model.Role
        //        });

        //        if (result)
        //            return RedirectToAction("ManageUsers", "SuperAdmin");

        //        ModelState.AddModelError("", "User creation failed.");
        //    }

        //    return View(model);
        //}

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
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState)
                {
                    Console.WriteLine($"Key: {modelError.Key}, Errors: {string.Join(", ", modelError.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new UpdateUserCommand
                {
                    UserId = model.Id,
                    FullName = model.FullName,
                    Email = model.Email,
                    Role = model.Role,
                   
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
