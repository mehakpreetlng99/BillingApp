
using BillingApp.DTO;
using BillingApp.Handlers.Authentication.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BillingApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new LoginUserCommand { Email = model.Email, Password = model.Password });
                if (result)
                {
                    // Fetch the user's role (this should be set correctly when the login process is successful)
                    var role = _httpContextAccessor.HttpContext.Session.GetString("UserRole");

                    // Redirect based on the role
                    if (role == "SuperAdmin")
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    if (role == "Admin")
                    {
                        return RedirectToAction("GetCategories", "Category");
                    }
                    if (role == "Agent")
                    {
                        return RedirectToAction("CreateInvoice", "BillingTest");
                    }

                    return RedirectToAction("TestSession", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Handle logout through the mediator or session
            await _mediator.Send(new LogoutUserCommand());

            // Clear session
            _httpContextAccessor.HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

//using BillingApp.DTO;
//using BillingApp.Handlers.Authentication.Commands;
//using BillingApp.Models;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace BillingApp.Web.Controllers
//{
//    public class AuthController:Controller
//    {
//        private readonly IMediator _mediator;


//        public AuthController(IMediator mediator)
//        {
//            _mediator = mediator;

//        }
//        public IActionResult Login() => View();

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginDTO model)
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await _mediator.Send(new LoginUserCommand { Email = model.Email, Password = model.Password });
//                if (result)
//                {
//                    var role = HttpContext.Session.GetString("UserRole");


//                    if (role == "SuperAdmin") return RedirectToAction("Index", "SuperAdmin");
//                    if (role == "Admin") return RedirectToAction("ManageProducts", "Product");
//                    if (role == "Agent") return RedirectToAction("Billing", "Billing");

//                    return RedirectToAction("TestSession", "Home");
//                }
//                ModelState.AddModelError("", "Invalid login attempt.");
//            }
//            return View(model);
//        }


//        public async Task<IActionResult> Logout()
//        {
//            await _mediator.Send(new LogoutUserCommand());
//            return RedirectToAction("Login");
//        }

//        public IActionResult AccessDenied()
//        {
//            return View();
//        }

//    }
//}
