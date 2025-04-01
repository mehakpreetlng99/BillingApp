using BillingApp.Handlers.Users.Commands;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Handlers.Users.Handlers
{
 
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(UserManager<User> userManager, ILogger<DeleteUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: " + request.UserId);
                    return false;
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User with ID {request.UserId} deleted successfully.");
                    return true;
                }

                _logger.LogWarning("Failed to delete user with ID: " + request.UserId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with ID {request.UserId}: {ex.Message}");
                throw;
            }
        }
    }

}
