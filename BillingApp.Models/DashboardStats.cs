using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Models
{
    public class DashboardStats
    {
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TodayInvoices { get; set; }
        public decimal TodayRevenue { get; set; }
        public List<RecentInvoice> RecentInvoices { get; set; } = new();
    }
}
