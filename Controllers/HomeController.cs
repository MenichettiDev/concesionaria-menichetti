using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authorization;

namespace concesionaria_menichetti.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UsuarioRepository _usuarioRepo;

    public HomeController(ILogger<HomeController> logger, UsuarioRepository usuarioRepo)
    {
        _logger = logger;
        _usuarioRepo = usuarioRepo;
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
    ///


}
