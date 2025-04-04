using BillingApp.Handlers.Users.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BillingApp.Handlers.Users.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(UserManager<User> userManager, ILogger<RegisterUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("User already exists.");
                    return false; 
                }
                var passwordValidator = _userManager.PasswordValidators.First();
                var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, null, request.Password);
                if (!passwordValidationResult.Succeeded)
                {
                    foreach (var error in passwordValidationResult.Errors)
                    {
                        _logger.LogWarning($"Password validation failed: {error.Description}");
                    }
                    return false; 
                }

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = request.FullName
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, request.Role);
                    _logger.LogInformation("User created successfully.");
                    return true;
                }

                _logger.LogWarning("User creation failed.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RegisterUserCommandHandler: {ex.Message}");
                throw;
            }
        }


        
    }
}


