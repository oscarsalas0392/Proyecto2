using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto2.ClasesBase;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Data.Interfaces;
using Proyecto2.Model;
using Proyecto2.Respuesta;
using Proyecto2.ViewModels;

namespace Proyecto2.Controllers
{
    public class UsuarioController : BaseController
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
            usuarioViewModel.artista = resArtista != null  ? resArtista.objecto : null;

            return View(usuarioViewModel);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(Prefix = "usuario")] Usuario usuario,[Bind(Prefix = "artista")] Artista artista, IFormFile? photo = null)
        {

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            if (id != usuario.Id)
            {
                return NotFound();
            }
           
            usuario.Contrasena = Usuario().Contrasena;
            usuario.TipoUsuario = Usuario().TipoUsuario;
            if (photo == null)
            {
                usuario.Foto = Usuario().Foto;
            }
            if (usuario.TipoUsuario != 1)
            {
                ModelState.Remove("artista.Usuario");
                ModelState.Remove("artista.Nombre");
                ModelState.Remove("artista.Informacion");
                ModelState.Remove("artista.Estilo");
                ModelState.Remove("artista.Experiencia");
            }

            ModelState.Remove("usuario.Contrasena");
            ModelState.Remove("artista.Enlace");
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        photo.CopyTo(ms);
                        string base64String = $"data:{photo.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
                        usuario.Foto = base64String;
                    }
                }
                Respuesta<Usuario> resUsuario = await _cRU.Actualizar(usuario);

                if (!resUsuario._estado || resUsuario._excepcion)
                {
                    usuarioViewModel.usuario = usuario;
                    usuarioViewModel.artista = artista;
                    return View(usuarioViewModel);
                }

                usuarioViewModel.usuario = usuario;

                if (Usuario().TipoUsuario == 1)
                {
                    Respuesta<Artista>? resArtista = await _cRA.ObtenerId(id);
                    artista.Id = resArtista.objecto.Id;
                    artista.Usuario = resArtista.objecto.Usuario;
                    artista.Enlace = $"{Request.Scheme}://{Request.Host.Value}{Request.Path.ToString().Replace("Edit", "Details")}";
                    resArtista = await _cRA.Actualizar(artista);
                    usuarioViewModel.artista = artista;

                    if (!resArtista._estado || resArtista._excepcion)
                    {                     
                        return View(usuarioViewModel);
                    }
                }
                return View(usuarioViewModel);
            }

            usuarioViewModel.usuario = usuario;
            usuarioViewModel.artista = artista;
            return View(usuarioViewModel);
        }

      
    }
}
