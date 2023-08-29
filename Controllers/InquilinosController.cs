
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class InquilinosController : Controller
{
    RepositorioInquilino ri = new RepositorioInquilino();

    private readonly ILogger<InquilinosController> _logger;

    public InquilinosController(ILogger<InquilinosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int page = 1, int pageSize = 10)
    {
        try
        {
            var inquilinos = ri.ObtenerInquilinos();
            int startIndex = (page - 1) * pageSize;
            var paginatedInquilino = inquilinos.Skip(startIndex).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)inquilinos.Count() / pageSize);

            return View(paginatedInquilino);
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

            ri.Alta(inquilino);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }

    public ActionResult Delete(int id)
    {
        var inquilino = ri.ObtenerPorId(id);
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.Mensaje = TempData["Mensaje"];
        if (TempData.ContainsKey("Error"))
            ViewBag.Error = TempData["Error"];
        return View(inquilino);
    }

    // POST: Inmueble/Eliminar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Inquilino inquilino)
    {
        try
        {
            ri.Baja(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.StackTrate = ex.StackTrace;
            return View(inquilino);
        }
    }
    public ActionResult Edit(int id)
    {
        var entidad = ri.ObtenerPorId(id);
        ViewBag.Inquilinos = ri.ObtenerInquilinos();
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.Mensaje = TempData["Mensaje"];
        if (TempData.ContainsKey("Error"))
            ViewBag.Error = TempData["Error"];
        return View(entidad);
    }

    // POST: Inmueble/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Inquilino inquilino)
    {
        try
        {
            inquilino.IdInquilino = id;
            ri.Editar(inquilino);
            TempData["Mensaje"] = "Datos guardados correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Propietarios = ri.ObtenerInquilinos();
            ViewBag.Error = ex.Message;
            ViewBag.StackTrate = ex.StackTrace;
            return View(inquilino);
        }
    }

    public ActionResult Details(int id)
    {
        var inquilino = ri.ObtenerPorId(id);
        return View(inquilino);
    }
}
