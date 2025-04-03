using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Data;
using BillingApp.Handlers.DashboardStat.Queries;
using BillingApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BillingApp.Handlers.DashboardStat.Handlers
{
    public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStats>
    {
        private readonly BillingDbContext _context;
        private readonly ILogger<GetDashboardStatsHandler> _logger;

        public GetDashboardStatsHandler(BillingDbContext context, ILogger<GetDashboardStatsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DashboardStats> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var today = DateTime.Today;

                var stats = new DashboardStats
                {
                    TotalSales = await _context.Invoices.CountAsync(cancellationToken),
                    TotalRevenue = await _context.Invoices.SumAsync(i => i.TotalAmount, cancellationToken),
                    TodayInvoices = await _context.Invoices
                        .CountAsync(i => i.Date.Date == today, cancellationToken),
                    TodayRevenue = await _context.Invoices
                        .Where(i => i.Date.Date == today)
                        .SumAsync(i => i.TotalAmount, cancellationToken),
                    RecentInvoices = await _context.Invoices
                        .OrderByDescending(i => i.Date)
                        .Take(5)
                        .Select(i => new RecentInvoice
                        {
                            Id = i.Id,
                            CustomerName = i.Customer.Name,
                            Amount = i.TotalAmount,
                            Date = i.Date
                        })
                        .ToListAsync(cancellationToken)
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dashboard stats");
                throw;
            }
        }
    }
}
