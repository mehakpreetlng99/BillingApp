using BillingApp.Handlers.DashboardStat.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Web.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DashboardStatsController : Controller
    {
        private readonly IMediator _mediator;

        public DashboardStatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var stats = await _mediator.Send(new GetDashboardStatsQuery());
            return View(stats);
        }
    }
}
