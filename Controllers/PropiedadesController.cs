using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Models;
using X.PagedList;
using X.PagedList.Mvc;

namespace Inmobiliaria2.Controllers
{
    public class PropiedadesController : Controller
    {

        private readonly RepositorioPropiedad repositorio = new RepositorioPropiedad();
        private readonly RepositorioPropietario repoPropietario = new RepositorioPropietario();

        private readonly ILogger<PropiedadesController> _logger;

        public PropiedadesController(ILogger<PropiedadesController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var propiedades = repositorio.ObtenerPropiedades();

                var paginatedPropiedades = propiedades.Skip((page - 1) * pageSize).Take(pageSize);

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)propiedades.Count() / pageSize);

                return View(paginatedPropiedades);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Propiedades/Details/5
        public ActionResult Details(int id)
        {
            var propiedad = repositorio.ObtenerPorId(id);
            return View(propiedad);
        }

        // GET: Propiedades/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
                return View();
            }
            catch
            {
                return View();
            }

        }

        // POST: Propiedades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propiedad propiedad)
        {
            try
            {

                repositorio.Alta(propiedad);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

        // GET: Propiedades/Edit/5
        public ActionResult Edit(int id)
        {
            var propiedad = repositorio.ObtenerPorId(id);
            ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(propiedad);
        }

        // POST: Propiedades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propiedad propiedad)
        {
            try
            {
                propiedad.IdPropiedad = id;
                repositorio.Editar(propiedad);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View(propiedad);
            }
        }

        // GET: Propiedades/Delete/5
        public ActionResult Delete(int id)
        {
            var propiedad = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(propiedad);
        }

        // POST: Propiedades/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propiedad propiedad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View(propiedad);
            }
        }
    }
}