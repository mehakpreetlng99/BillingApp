using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.DTO;
using BillingApp.Handlers.Customers.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Handlers.Customers.Handlers
{
    public class GetCustomerByPhoneHandler : IRequestHandler<GetCustomerByPhoneQuery, CustomerDTO>
    {
        private readonly BillingDbContext _context;

        public GetCustomerByPhoneHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerDTO> Handle(GetCustomerByPhoneQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (customer == null) return null;

            return new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber
            };
        }
    }
}
