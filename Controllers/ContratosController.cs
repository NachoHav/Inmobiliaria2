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
            return View();
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
            return View();
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}