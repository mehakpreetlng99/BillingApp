using BillingApp.Models;
using MediatR;


namespace BillingApp.Handlers.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<List<Category>>
    {
    }
}
