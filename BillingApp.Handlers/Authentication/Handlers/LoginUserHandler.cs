using BillingApp.Handlers.Authentication.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace BillingApp.Handlers.Authentication.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, bool>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LoginUserHandler> _logger;

        public LoginUserHandler(SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ILogger<LoginUserHandler> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any())
                    {
                        var role = roles.First();

                        // Add role as a session
                        Console.WriteLine($"✅ User role retrieved: {roles.First()}");
                        _httpContextAccessor.HttpContext.Session.SetString("UserRole", role);

                        // 🔹 Add role as a claim
                        var claims = new List<Claim> { new Claim(ClaimTypes.Role, role) };
                        var identity = new ClaimsIdentity(claims, "Cookies");
                        var principal = new ClaimsPrincipal(identity);
                        await _httpContextAccessor.HttpContext.SignInAsync("Cookies", principal);
                    }
                    else
                    {
                        Console.WriteLine("⚠️ User has no roles assigned.");
                    }
                    return true;
                }
            }
            return false;
        }

    }
}