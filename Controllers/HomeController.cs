using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RepositorioPropiedad repositorioPropiedad = new();
    private readonly RepositorioPropietario repositorioPropietario = new();
    private readonly RepositorioContrato repositorioContrato = new();
    private readonly RepositorioInquilino repositorioInquilino = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {

        int totalPropiedades = repositorioPropiedad.ObtenerNumeroTotalPropiedades();
        int propiedadesDisponibles = repositorioPropiedad.ObtenerNumeroPropiedadesDisponibles();

        int totalContratosActivos = repositorioContrato.ObtenerNumeroTotalContratosActivos();
        int contratosVencenProximamente = repositorioContrato.ObtenerNumeroContratosVencenProximamente(4);

        int totalInquilinosRegistrados = repositorioInquilino.ObtenerNumeroTotalInquilinos();


        ViewBag.TotalPropiedades = totalPropiedades;
        ViewBag.PropiedadesDisponibles = propiedadesDisponibles;

        ViewBag.TotalContratosActivos = totalContratosActivos;
        ViewBag.ContratosVencenProximamente = contratosVencenProximamente;

        ViewBag.TotalInquilinosRegistrados = totalInquilinosRegistrados;


        return View();
    }

    public ActionResult Denegado()
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
}
