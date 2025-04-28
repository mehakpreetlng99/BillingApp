using BillingApp.DTO;
using MediatR;
using System.Collections.Generic;

namespace BillingApp.Handlers.Subcategories.Queries
{
    public class GetSubcategoriesByCategoryIdQuery : IRequest<List<SubcategoryDTO>>
    {
        public int CategoryId { get; set; }
        public GetSubcategoriesByCategoryIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
