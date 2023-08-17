
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class PropietariosController : Controller
{
    private readonly ILogger<PropietariosController> _logger;

    public PropietariosController(ILogger<PropietariosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            RepositorioPropietario rp = new RepositorioPropietario();
            var propietarios = rp.ObtenerPropietarios();
            return View(propietarios);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Propietario propietario)
    {
        try
        {
            RepositorioPropietario rp = new RepositorioPropietario();
            rp.Alta(propietario);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }
}
