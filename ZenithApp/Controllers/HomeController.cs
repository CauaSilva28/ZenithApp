using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenithApp.Models;

namespace ZenithApp.Controllers;

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

    public IActionResult Cadastro()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Termos()
    {
        return View();
    }

    public IActionResult Atleta()
    {
        return View();
    }

    public IActionResult Treinador()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
