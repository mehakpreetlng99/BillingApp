
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
                    
                    var role = _httpContextAccessor.HttpContext.Session.GetString("UserRole");

                   
                    if (role == "SuperAdmin")
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    if (role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
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
            
            await _mediator.Send(new LogoutUserCommand());

            _httpContextAccessor.HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

