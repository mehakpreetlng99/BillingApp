
using System.ComponentModel.DataAnnotations;
using BillingApp.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Handlers.Invoices.Commands
{
    public class CreateInvoiceCommand : IRequest<int>
    {

        [Required(ErrorMessage = "Customer phone is required")]
        public string CustomerPhone { get; set; }

        public decimal? DiscountPercentage { get; set; }
        public decimal?  GSTPercentage{get;set;}

        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; }

        public decimal TotalAmount { get; set; }

        public List<InvoiceItemDTO> Items { get; set; } = new();
        
    }
}
