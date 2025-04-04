using BillingApp.Common.Constants;
using System.Security.Claims;
using BillingApp.DTO;
using BillingApp.Handlers.Users.Commands;
using BillingApp.Handlers.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Web.Controllers
{

    

    [Authorize(Policy = "RequireSuperAdminRole")]
    public class SuperAdminController : Controller
    {
        private readonly IMediator _mediator;
    

        public SuperAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
            
            var passwordError = ValidatePassword(model.Password);
            if (!string.IsNullOrEmpty(passwordError))
            {
                ModelState.AddModelError("Password", passwordError);
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new RegisterUserCommand
                {
                    
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = string.IsNullOrEmpty(model.Role) ? "Agent" : model.Role 
                });

                if (result)
                {
                    TempData["AlertMessage"] = "User Added successfully!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction("ManageUsers", "SuperAdmin");
                }
                else
                {
                    ModelState.AddModelError("", "User already exists or invalid data.");
                }
            }
            else
            {
                
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
        private string ValidatePassword(string password)
        {
            var specialChars = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";
            if (password.Length < 8 ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsDigit) ||
                !password.Any(c => specialChars.Contains(c)))
            {
                return "Password must be at least 8 characters, 1 uppercase letter, 1 special character, 1 number";
            }
            return null; 
        }
        public async Task<IActionResult> ManageAgents()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetUsersByRoleQuery
            {
                Role = UserRoles.Agent,
                RequestingUserId = currentUserId,
                RequestingUserRole = UserRoles.SuperAdmin
            };

            var agents = await _mediator.Send(query);
            return View(agents);
        }
        
        public async Task<IActionResult> ManageAdmins()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetUsersByRoleQuery
            {
                Role = UserRoles.Admin,
                RequestingUserId = currentUserId,
                RequestingUserRole = UserRoles.SuperAdmin
            };

            var admins = await _mediator.Send(query);
            return View(admins);
        }

        

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand(userId));

            if (result)
            {
                TempData["AlertMessage"] = "User Deleted successfully!";
                TempData["AlertType"] = "success";
                return RedirectToAction("ManageUsers", "SuperAdmin");
            }
            
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
                {
                    TempData["AlertMessage"] = "User updated successfully!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction("ManageUsers", "SuperAdmin");
                }

                ModelState.AddModelError("", "User update failed.");
            }

            return View(model);
        }

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

       
    }


}
