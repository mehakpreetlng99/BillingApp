
using BillingApp.DTO;
using BillingApp.Handlers.Authentication.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace BillingApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthController(IMediator mediator, IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,IEmailSender emailSender)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailSender = emailSender;
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToAction("ForgotPassword", "Auth");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPasswordConfirmation", "Auth",
                    new { token }, protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset your password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return RedirectToAction("ForgotPasswordConfirmation", "Auth");
            }
            return View(model);
        }
    }
}

