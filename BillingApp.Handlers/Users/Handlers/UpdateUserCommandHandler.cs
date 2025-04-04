
using BillingApp.Handlers.Users.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Users.Handlers
{
    
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(UserManager<User> userManager, ILogger<UpdateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: " + request.UserId);
                    return false;
                }

                user.FullName = request.FullName;
                user.Email = request.Email;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, request.Password);
                    if (!passwordResult.Succeeded)
                    {
                        _logger.LogWarning("Password update failed for user ID: " + request.UserId);
                        return false;
                    }
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, request.Role);

                    _logger.LogInformation($"User with ID {request.UserId} updated successfully.");
                    return true;
                }

                _logger.LogWarning("Failed to update user with ID: " + request.UserId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with ID {request.UserId}: {ex.Message}");
                throw;
            }
        }




    }

}
