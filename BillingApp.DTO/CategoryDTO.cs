

using System.ComponentModel.DataAnnotations;

namespace BillingApp.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
