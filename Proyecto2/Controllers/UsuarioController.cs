using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Data.Interfaces;
using Proyecto2.Model;
using Proyecto2.Respuesta;
using Proyecto2.ViewModels;

namespace Proyecto2.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Context _context;
        private readonly UsuarioRepositorio _cRU;
        private readonly ArtistaRepositorio _cRA;
        public UsuarioController(Context context, UsuarioRepositorio cRU, ArtistaRepositorio cRA)
        {
            _context = context;
            _cRU = cRU;
            _cRA = cRA;
        }


        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
     
            Respuesta<Artista>? resArtista = null;
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            Respuesta<Usuario> resUsuario = await _cRU.ObtenerId(id);

            if (resUsuario?.objecto?.TipoUsuario == 1)
            {
                resArtista = await _cRA.ObtenerId(id);
            }


            if (resUsuario is null || !resUsuario._estado || resUsuario._excepcion)
            {
                return NotFound();
            }

            usuarioViewModel.usuario = resUsuario.objecto;
            usuarioViewModel.artista = resArtista.objecto;

            return View(usuarioViewModel);
        }



        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Respuesta<Artista>? resArtista = null;
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            Respuesta<Usuario> resUsuario = await _cRU.ObtenerId(id);

            if (resUsuario?.objecto?.TipoUsuario == 1)
            {
                resArtista = await _cRA.ObtenerId(id);
            }

            if (resUsuario is null || !resUsuario._estado || resUsuario._excepcion)
            {
                return NotFound();
            }

            usuarioViewModel.usuario = resUsuario.objecto;
            usuarioViewModel.artista = resArtista.objecto;

            return View(usuarioViewModel);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Correo,TipoUsuario,Foto")] UsuarioViewModel usuarioViewModel)
        {
            if (id != usuarioViewModel.usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Respuesta<Usuario> resUsuario = await _cRU.Actualizar(usuarioViewModel.usuario);

                if (!resUsuario._estado || resUsuario._excepcion)
                {
                    return View(usuarioViewModel);
                }

                if(usuarioViewModel.usuario.TipoUsuario == 1)
                {
                    Respuesta<Artista> resArtista = await _cRA.Actualizar(usuarioViewModel.artista);

                    if (!resArtista._estado || resArtista._excepcion)
                    {
                        return View(usuarioViewModel);
                    }
                }

                return RedirectToAction(nameof(Index));
            }


            return View(usuarioViewModel);
        }

      
    }
}
