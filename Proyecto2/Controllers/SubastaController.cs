using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto2.ClasesBase;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Data.Interfaces;
using Proyecto2.Extensions;
using Proyecto2.Model;
using Proyecto2.Respuesta;
using Proyecto2.ViewModels;

namespace Proyecto2.Controllers
{
    public class SubastaController : BaseController
    {
        private readonly Context _context;
        private readonly IRepositorio<Subasta, int?> _cR;
        private readonly SubastaRepositorio _cR2;
        private readonly ObraArteRepositorio _cROA;


        public SubastaController(Context context, IRepositorio<Subasta, int?> cR, SubastaRepositorio cR2, ObraArteRepositorio cROA)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
            _cROA = cROA;
        }

        // GET: Subasta
        public async Task<IActionResult> Index(IndexViewModel<Subasta, SubastaRepositorio, int?> vm)
        {
            vm.paginacion.columnas["Artista"] = "ObraArteNavigation.Artista";
            await vm.HandleRequest(_cR2, "ObraArteNavigation.Titulo", "ObraArteNavigation.Titulo", artista:Artista().Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: Subasta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Respuesta<Subasta> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }

            return View(notificacion.objecto);
        }


        public async Task DevolverObraArte()
        {
            Filtro filtro = new Filtro();
            filtro.artista = Artista().Id;
            Respuesta<ObraArte> respuestaObraArte = await _cROA.ObtenerLista(filtro);
            ViewData["ObraArte"] = new SelectList(respuestaObraArte.lista, "Id", "Titulo");
        }
        // GET: Subasta/Create
        public async Task<IActionResult> Create()
        {
            await DevolverObraArte();
            return View();
        }

        // POST: Subasta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ObraArte,PrecioInicial,PrecioActual,FechaInicial,FechaCierre,Eliminado")] Subasta subasta)
        {

            subasta.PrecioActual = subasta.PrecioInicial;


            Respuesta<bool> respuestaExiste = _cR2.ExisteSubasta(subasta.ObraArte);

            ModelState.Remove("Id");
            if (ModelState.IsValid && !respuestaExiste.objecto)
            {

                Respuesta<Subasta> respuesta = await _cR.Guardar(subasta);
                return RedirectToAction(nameof(Index));
            }

            await DevolverObraArte();

            return View(subasta);
        }

        // GET: Subasta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Respuesta<Subasta> respuesta = await _cR.ObtenerId(id);

            if (!respuesta._estado || respuesta._excepcion || respuesta.objecto is null)
            {
                return NotFound();
            }

            await DevolverObraArte();

            return View(respuesta.objecto);
        }

        // POST: Subasta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ObraArte,PrecioInicial,PrecioActual,FechaInicial,FechaCierre,Eliminado")] Subasta subasta)
        {

            if (id != subasta.Id)
            {
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                Respuesta<Subasta> notificacion = await _cR.Actualizar(subasta);

                if (!notificacion._estado || notificacion._excepcion)
                {
               
                    return View(subasta);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(subasta);
        }

        // GET: Subasta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Respuesta<Subasta> notificacion = await _cR.ObtenerId(id);

            if (!notificacion._estado || notificacion._excepcion)
            {
                return NotFound();
            }
            return View(notificacion.objecto);
        }

        // POST: Subasta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Respuesta<Subasta> notificacion = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
