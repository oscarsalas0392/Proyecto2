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
    public class TransaccionController : BaseController
    {
        private readonly Context _context;
        private readonly IRepositorio<Transaccion, int?> _cR;
        private readonly TransaccionRepositorio _cR2;
        private readonly OfertaRepositorio _cRO;
        public TransaccionController(Context context, IRepositorio<Transaccion, int?> cR, TransaccionRepositorio cR2, OfertaRepositorio cRO)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
            _cRO= cRO;
        }

        // GET: Transaccion
        public async Task<IActionResult> Index(IndexViewModel<Transaccion, TransaccionRepositorio, int?> vm)
        {

            await vm.HandleRequest(_cR2, "SubastaNavigation.ObraArteNavigation.Titulo", "SubastaNavigation.ObraArteNavigation.Titulo", Usuario().Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: Transaccion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transaccion
                .Include(t => t.OfertaNavigation)
                .Include(t => t.SubastaNavigation)
                .Include(t => t.UsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return View(transaccion);
        }

        // GET: Transaccion/Create
        public async Task<IActionResult> Create(int id)
        {
             Respuesta<Oferta> respuesta =  await _cRO.ObtenerId(id);

            Transaccion transaccion = new Transaccion();
            transaccion.Usuario = respuesta.objecto.Usuario;
            transaccion.Subasta = respuesta.objecto.Subasta;
            transaccion.Oferta = respuesta.objecto.Id;

            return View(transaccion);
        }

        // POST: Transaccion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuario,Subasta,Oferta,Tarjeta,Fecha,Eliminado")] Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                transaccion.Fecha = DateTime.Now;
                Respuesta<Transaccion> respuesta = await _cR.Guardar(transaccion);
                return RedirectToAction(nameof(Index));
            }

            return View(transaccion);
        }

        // GET: Transaccion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            ViewData["Oferta"] = new SelectList(_context.Oferta, "Id", "Id", transaccion.Oferta);
            ViewData["Subasta"] = new SelectList(_context.Subasta, "Id", "Id", transaccion.Subasta);
            ViewData["Usuario"] = new SelectList(_context.Usuario, "Id", "Contrasena", transaccion.Usuario);
            return View(transaccion);
        }

        // POST: Transaccion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Subasta,Oferta,Tarjeta,Fecha,Eliminado")] Transaccion transaccion)
        {
            if (id != transaccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccionExists(transaccion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Oferta"] = new SelectList(_context.Oferta, "Id", "Id", transaccion.Oferta);
            ViewData["Subasta"] = new SelectList(_context.Subasta, "Id", "Id", transaccion.Subasta);
            ViewData["Usuario"] = new SelectList(_context.Usuario, "Id", "Contrasena", transaccion.Usuario);
            return View(transaccion);
        }

        // GET: Transaccion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transaccion
                .Include(t => t.OfertaNavigation)
                .Include(t => t.SubastaNavigation)
                .Include(t => t.UsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return View(transaccion);
        }

        // POST: Transaccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccion = await _context.Transaccion.FindAsync(id);
            if (transaccion != null)
            {
                _context.Transaccion.Remove(transaccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccionExists(int id)
        {
            return _context.Transaccion.Any(e => e.Id == id);
        }
    }
}
