
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class InquilinosController : Controller
{
    private readonly ILogger<InquilinosController> _logger;

    public InquilinosController(ILogger<InquilinosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            RepositorioInquilino ri = new RepositorioInquilino();
            var inquilinos = ri.ObtenerInquilinos();
            return View(inquilinos);
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
    public IActionResult Create(Inquilino inquilino)
    {
        try
        {
            RepositorioInquilino ri = new RepositorioInquilino();
            ri.Alta(inquilino);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }
}
