using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Foreign Key
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; } // Navigation Property
    }

}
