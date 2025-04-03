using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.DTO
{
    public class InvoiceDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Customer phone is required")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; }

        public decimal TotalAmount { get; set; }

        [Range(0, 100)]
        public decimal? DiscountPercentage { get; set; }  

        public decimal? DiscountAmount { get; set; }     

        [Range(0, 100)]
        public decimal? GSTPercentage { get; set; }      

        public decimal? GSTAmount { get; set; }          

        public decimal? Subtotal { get; set; }

        //public string AgentId { get; set; }

        public List<InvoiceItemDTO> Items { get; set; } = new List<InvoiceItemDTO>();

        public DateTime Date { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        

        //public int Id { get; set; }
        //public string CustomerPhone { get; set; }
        public int CustomerId { get; set; }
        //public string CustomerName { get; set; }
        //public string PhoneNumber { get; set; }
        //public decimal TotalAmount { get; set; }
        //public int AgentId { get; set; }
        //public List<InvoiceItemDTO> Items { get; set; }
        //public DateTime Date { get; set; }
        //public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }

    }

