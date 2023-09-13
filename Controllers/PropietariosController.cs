
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class PropietariosController : Controller
{

    private readonly RepositorioPropietario repositorio = new RepositorioPropietario();

    private readonly ILogger<PropietariosController> _logger;

    public PropietariosController(ILogger<PropietariosController> logger)
    {
        _logger = logger;
    }

    public ActionResult Index(int page = 1, int pageSize = 9)
    {
        try
        {
            var prop = repositorio.ObtenerPropietarios();

            int startIndex = (page - 1) * pageSize;
            var paginatedProp = prop.Skip(startIndex).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)prop.Count() / pageSize);

            return View(paginatedProp);
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

            repositorio.Alta(propietario);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }

    public ActionResult Edit(int id)
    {
        var propietario = repositorio.ObtenerPorId(id);
        ViewBag.Propietarios = repositorio.ObtenerPropietarios();
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.Mensaje = TempData["Mensaje"];
        if (TempData.ContainsKey("Error"))
            ViewBag.Error = TempData["Error"];

        return View(propietario);
    }

    // POST: Inmueble/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Propietario propietario)
    {
        try
        {
            propietario.IdPropietario = id;
            repositorio.Editar(propietario);
            TempData["Mensaje"] = "Datos guardados correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Propietarios = repositorio.ObtenerPropietarios();
            ViewBag.Error = ex.Message;
            ViewBag.StackTrate = ex.StackTrace;
            return View(propietario);
        }
    }

    public ActionResult Delete(int id)
    {
        var propietario = repositorio.ObtenerPorId(id);
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.Mensaje = TempData["Mensaje"];
        if (TempData.ContainsKey("Error"))
            ViewBag.Error = TempData["Error"];
        return View(propietario);
    }

    // POST: Inmueble/Eliminar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Propietario propietario)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.StackTrate = ex.StackTrace;
            return View(propietario);
        }
    }

    public ActionResult Details(int id)
    {
        var propietario = repositorio.ObtenerPorId(id);
        return View(propietario);
    }


    public IActionResult Search(string searchTerm)
    {
        var propietarios = repositorio.ObtenerPropietarios();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            propietarios = propietarios.Where(p =>
                p.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Apellido.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            // Limitar a 9 resultados
        }
        var paginatedPropietarios = propietarios.Take(9);
        return PartialView("_PropietarioTablePartial", paginatedPropietarios);
    }
}
