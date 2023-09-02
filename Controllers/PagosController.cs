using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Models;

namespace Inmobiliaria2.Controllers
{
    public class PagosController : Controller
    {

        private readonly RepositorioPago repositorioPago = new RepositorioPago();
        private readonly RepositorioContrato repositorioContrato = new RepositorioContrato();
        private readonly ILogger<PagosController> _logger;
        public PagosController(ILogger<PagosController> logger)
        {
            _logger = logger;
        }

        // GET: Pagos
        public ActionResult Index(int id)
        {
            try
            {
                var pagos = repositorioPago.ObtenerPagos(id);
                ViewBag.idContrato = id;

                return View(pagos);


            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Pagos/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var pago = repositorioPago.ObtenerPago(id);
                return View(pago);
            }
            catch (Exception e)
            {
                return View(e.Message);
            }

        }

        // GET: Pagos/Create
        public ActionResult Create(int id)
        {
            try
            {
                ViewBag.Contrato = repositorioContrato.ObtenerPorId(id);
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                var res = repositorioPago.Alta(pago);
                TempData["Mensaje"] = "Pago registrado correctamente";


                return RedirectToAction(nameof(Index), new { id = pago.ContratoId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Edit/5
        public ActionResult Edit(int id)
        {
            var pago = repositorioPago.ObtenerPago(id);
            ViewBag.Contrato = repositorioContrato.ObtenerPorId(pago.ContratoId);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(pago);

        }
        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                // TODO: Add update logic here
                pago.IdPago = id;
                repositorioPago.Editar(pago);
                TempData["Mensaje"] = "Pago actualizado correctamente";
                return RedirectToAction(nameof(Index), new { id = pago.ContratoId });
            }
            catch (Exception e)
            {
                ViewBag.Contrato = repositorioContrato.ObtenerPorId(pago.ContratoId);
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View(pago);
            }
        }

        // GET: Pagos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pagos/Delete/5
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