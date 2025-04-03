using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Customers.Queries
{
    public class GetCustomerByPhoneQuery : IRequest<CustomerDTO>
    {
        public string PhoneNumber { get; set; }

        public GetCustomerByPhoneQuery(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
