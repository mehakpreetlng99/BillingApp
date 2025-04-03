using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? AgentId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTAmount { get; set; }
        public decimal Subtotal { get; set; }
        public Customer Customer { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }

}
