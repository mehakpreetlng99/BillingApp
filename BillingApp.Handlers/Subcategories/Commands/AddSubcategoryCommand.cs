
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Subcategories.Commands
{
    public class AddSubcategoryCommand : IRequest<bool>
    {
        public SubcategoryDTO Subcategory { get; set; }
        public AddSubcategoryCommand(SubcategoryDTO subcategory)
        {
           
            if (string.IsNullOrWhiteSpace(subcategory.Name))
            {
                throw new ArgumentException("Subcategory Name is required.");
            }
            if (subcategory.Name.Length > 50)
            {
                throw new ArgumentException("Subcategory Name cannot exceed 50 characters.");
            }
            if (subcategory.CategoryId <= 0)
            {
                throw new ArgumentException("Category is required.");
            }

            Subcategory = subcategory;
        }
    }
}
