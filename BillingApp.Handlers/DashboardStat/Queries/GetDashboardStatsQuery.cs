using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApp.Models;
using MediatR;

namespace BillingApp.Handlers.DashboardStat.Queries
{
    public record GetDashboardStatsQuery : IRequest<DashboardStats>;
}
