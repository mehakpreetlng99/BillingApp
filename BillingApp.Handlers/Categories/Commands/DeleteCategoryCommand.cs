using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Handlers.Categories.Commands
{
   public  class DeleteCategoryCommand:IRequest<bool>
    {
        public int Id { get; set; }
    }
}
