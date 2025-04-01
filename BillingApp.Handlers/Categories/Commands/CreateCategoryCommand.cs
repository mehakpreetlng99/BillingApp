using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BillingApp.Handlers.Categories.Commands
{
   public class CreateCategoryCommand:IRequest<bool>
    {
        [Required(ErrorMessage = "Category Name is required.")]
        public string Name { get; set; }
    }
}
