using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Handlers.Authentication.Commands
{
   public class LoginUserCommand:IRequest<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
