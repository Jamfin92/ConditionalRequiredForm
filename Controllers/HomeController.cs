using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConditionalRequiredForm.Models;

namespace ConditionalRequiredForm.Controllers;

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

    public IActionResult ConditionalForm()
    {
        return View(new ConditionalFormModel());
    }

    [HttpPost]
    public IActionResult ConditionalForm(ConditionalFormModel model)
    {
        if (ModelState.IsValid)
        {
            TempData["Success"] = "Form submitted successfully!";
            return RedirectToAction("ConditionalForm");
        }
        return View(model);
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
}
