using BillingApp.DTO;
using MediatR;


namespace BillingApp.Handlers.Users.Queries
{
    
    public class GetUsersQuery : IRequest<List<UserDTO>> { }

}
