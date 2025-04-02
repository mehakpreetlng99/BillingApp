using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BillingApp.Handlers.Subcategories.Commands
{
    public class DeleteSubcategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
