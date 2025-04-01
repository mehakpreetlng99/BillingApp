using BillingApp.Handlers.Authentication.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Authentication.Handlers
{
    public class LogoutUserHandler:IRequestHandler<LogoutUserCommand>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LogoutUserHandler> _logger;

        public LogoutUserHandler(SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, ILogger<LogoutUserHandler> logger)
        {
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            _httpContextAccessor.HttpContext.Session.Clear();
            _logger.LogInformation("User logged out successfully.");
            return Unit.Value;
        }
    }
}
