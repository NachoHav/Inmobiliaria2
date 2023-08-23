
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

}
