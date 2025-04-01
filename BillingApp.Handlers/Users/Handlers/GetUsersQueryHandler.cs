

using BillingApp.DTO;
using BillingApp.Handlers.Users.Queries;
using BillingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.Users.Handlers
{
   
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDTO>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        public GetUsersQueryHandler(UserManager<User> userManager, ILogger<GetUsersQueryHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<List<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = _userManager.Users.ToList();

                var userDTOs = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = string.Join(", ", _userManager.GetRolesAsync(u).Result)
                }).ToList();

                return userDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching users: {ex.Message}");
                throw;
            }
        }
    }

}
