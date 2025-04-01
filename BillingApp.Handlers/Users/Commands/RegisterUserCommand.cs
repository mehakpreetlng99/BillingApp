using MediatR;


namespace BillingApp.Handlers.Users.Commands
{

    public class RegisterUserCommand : IRequest<bool>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
