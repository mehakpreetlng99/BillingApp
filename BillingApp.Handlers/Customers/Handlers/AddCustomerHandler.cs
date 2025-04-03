
using BillingApp.Data;
using BillingApp.Handlers.Customers.Commands;
using BillingApp.Models;
using MediatR;

namespace BillingApp.Handlers.Customers.Handlers
{
    public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, int>
    {
        private readonly BillingDbContext _context;

        public AddCustomerHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Customer.Name,
                PhoneNumber = request.Customer.PhoneNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }
    }
}