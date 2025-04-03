using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.DTO;
using MediatR;

namespace BillingApp.Handlers.Invoices.Queries
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDTO>
    {
        public int InvoiceId { get; set; }

        public GetInvoiceByIdQuery(int invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
