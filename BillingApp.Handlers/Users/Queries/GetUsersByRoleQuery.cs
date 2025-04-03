
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Users.Queries
{
    public class GetUsersByRoleQuery : IRequest<List<UserDTO>>
    {
        public string Role { get; set; }
        public string RequestingUserId { get; set; }
        public string RequestingUserRole { get; set; }
    }
}
