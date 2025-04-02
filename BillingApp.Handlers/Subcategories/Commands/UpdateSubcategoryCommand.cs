using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Subcategories.Commands
{

    public class UpdateSubcategoryCommand : IRequest<bool>
    {
        public SubcategoryDTO Subcategory { get; set; }
    }
}
