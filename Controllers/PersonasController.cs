using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class PersonasController : Controller
{
    private readonly ILogger<PersonasController> _logger;

    public PersonasController(ILogger<PersonasController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioPersona rp = new RepositorioPersona();
        List<Persona> personas = rp.ObtenerPersonas();
        return View(personas);

    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Persona persona)
    {
        try
        {
            RepositorioPersona rp = new RepositorioPersona();
            rp.Alta(persona);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }


}
