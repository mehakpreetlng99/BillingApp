using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Subcategories.Queries
{
    public class GetSubcategoriesQuery : IRequest<List<SubcategoryDTO>> { }
}
