
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace test.Controllers;

public class PropietariosController : Controller
{

    private readonly RepositorioPropietario repositorio;

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

    public ActionResult Edit(int id)
    {
        try
        {
            var propietario = repositorio.ObtenerPropietario(id);
            return View(propietario);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpPost]
    public ActionResult Edit(int id, Propietario propietario)
    {
        Propietario p = null;
        try
        {
            p = repositorio.ObtenerPropietario(id);
            p.Nombre = propietario.Nombre;
            p.Apellido = propietario.Apellido;
            p.Dni = propietario.Dni;
            p.Telefono = propietario.Telefono;
            p.Email = propietario.Email;
            repositorio.Modificacion(p);
            TempData["Mensaje"] = "Datos guardados!";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            throw;
        }
    }

    public ActionResult Delete()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Ocurrió un error al intentar eliminar el propietario.";
            return View(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id)
    {
        try
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}
