using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto2.Data;
using Proyecto2.Model;

namespace Proyecto2.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransaccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaccion
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transaccion.Include(t => t.OfertaNavigation).Include(t => t.SubastaNavigation).Include(t => t.UsuarioNavigation);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create(int id)
        {
            ViewData["Oferta"] = new SelectList(_context.Oferta, "Id", "Id");
            ViewData["Subasta"] = new SelectList(_context.Subasta, "Id", "Id");
            ViewData["Usuario"] = new SelectList(_context.Usuario, "Id", "Contrasena");
            return View();
        }

        // POST: Transaccion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Usuario,Subasta,Oferta,Tarjeta,Fecha,Eliminado")] Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Oferta"] = new SelectList(_context.Oferta, "Id", "Id", transaccion.Oferta);
            ViewData["Subasta"] = new SelectList(_context.Subasta, "Id", "Id", transaccion.Subasta);
            ViewData["Usuario"] = new SelectList(_context.Usuario, "Id", "Contrasena", transaccion.Usuario);
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
