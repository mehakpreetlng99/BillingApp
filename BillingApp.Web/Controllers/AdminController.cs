using BillingApp.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApp.Web.Controllers
{
    // Controllers/AdminController.cs
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Other actions will be added here
    }
}
