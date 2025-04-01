using BillingApp.DTO;
using MediatR;


namespace BillingApp.Handlers.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDTO>
    {
        public string UserId { get; set; }

        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
