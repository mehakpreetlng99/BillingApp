using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BillingApp.Web.Models;

namespace BillingApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult TestSession()
    {
        var role = HttpContext.Session.GetString("UserRole");
        return Content($"Stored Role: {role ?? "None"}");
    }
}


