using MediatR;

namespace BillingApp.Handlers.Users.Commands
{
    
    public class UpdateUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? Password { get; set; }
    }

}
