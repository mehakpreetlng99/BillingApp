using BillingApp.DTO;
using BillingApp.Handlers.Users.Queries;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BillingApp.Handlers.Users.Handlers
{
    
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(UserManager<User> userManager, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user == null)
                    throw new Exception("User not found.");

                return new UserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = string.Join(", ", await _userManager.GetRolesAsync(user))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user by ID: {ex.Message}");
                throw;
            }
        }
    }

}
