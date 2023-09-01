using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace Inmobiliaria2.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repositorioContrato = new RepositorioContrato();
        private readonly RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
        private readonly RepositorioPropiedad repositorioPropiedad = new RepositorioPropiedad();
        private readonly ILogger<ContratosController> _logger;

        public ContratosController(ILogger<ContratosController> logger)
        {
            _logger = logger;
        }
        // GET: Contratos
        public ActionResult Index()
        {
            try
            {
                var contratos = repositorioContrato.ObtenerContratos();
                return View(contratos);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            var contrato = repositorioContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();
                ViewBag.Propiedades = repositorioPropiedad.ObtenerPropiedades();
                return View();
            }
            catch
            {
                return View();
            }

        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                // TODO: Add insert logic here
                repositorioContrato.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            var contrato = repositorioContrato.ObtenerPorId(id);
            ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();
            ViewBag.Propiedades = repositorioPropiedad.ObtenerPropiedades();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                // TODO: Add update logic here
                contrato.IdContrato = id;
                repositorioContrato.Editar(contrato);
                TempData["Mensaje"] = "Edición realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Inquilinos = repositorioInquilino.ObtenerInquilinos();
                ViewBag.Propiedades = repositorioPropiedad.ObtenerPropiedades();
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View(contrato);
            }
        }

        // GET: Contratos/Delete/5
        public ActionResult Delete(int id)
        {
            var contrato = repositorioContrato.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                repositorioContrato.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View(contrato);
            }
        }
    }
}