using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Models;

namespace BillingApp.DTO
{

    public class SubcategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Subcategory Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category selection is required.")]
        public int CategoryId { get; set; }

        
       
    }
}
