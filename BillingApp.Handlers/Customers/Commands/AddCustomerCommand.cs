using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Customers.Commands
{
    public class AddCustomerCommand : IRequest<int>
    {
        public CustomerDTO Customer { get; }

        public AddCustomerCommand(CustomerDTO customer)
        {
            Customer = customer;
        }
    }
}
