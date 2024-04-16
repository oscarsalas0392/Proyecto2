using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Data.Interfaces;
using Proyecto2.Model;
using Proyecto2.Respuesta;

namespace Proyecto2.Controllers
{

    public class AccesoController : Controller
    {
        private readonly Context _context;
        private readonly UsuarioRepositorio _cRU;
        private readonly ArtistaRepositorio _cRA;
        private readonly TipoUsuarioRepositorio _cRTU;
        public AccesoController(Context context, UsuarioRepositorio cRU, TipoUsuarioRepositorio cRTU, ArtistaRepositorio cRA)
        {
            _context = context;
            _cRU = cRU;
            _cRTU = cRTU;
            _cRA = cRA;
        }
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Registrar()
        {
            Respuesta<TipoUsuario> respuesta = await _cRTU.ObtenerLista();
            ViewData["TipoUsuario"] = new SelectList(respuesta.lista, "Id", "Descripcion");
            return View();
       }


        [HttpPost]
        public async Task<ActionResult> Login(Usuario oUsuario)
        {
            Respuesta<Usuario> notificacion = await _cRU.ValidarUsuario(oUsuario.Correo, oUsuario.Contrasena);
            Usuario? usuario = notificacion.objecto;

            if (usuario is null)
            {
                TempData["MensajeError"] = "El usuario o la contraseña no son correctos";
            }

            else
            {

                HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(notificacion.objecto));
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(Usuario oUsuario)
        {
            Respuesta<Usuario> respuesta = await _cRU.Guardar(oUsuario);

            if (!respuesta._estado || respuesta._excepcion || respuesta.objecto is null)
            {
                TempData["MensajeError"] = respuesta.mensaje.Descripcion;
            }
            else
            {

                if(oUsuario.TipoUsuario == 1)
                {
                    Artista artista = new Artista();
                    artista.Usuario = respuesta.objecto.Id;
                    artista.Informacion = "";
                    artista.Experiencia = "";
                    artista.Enlace = "";
                    artista.Nombre = "";
                    artista.Estilo = "";
                    Respuesta<Artista> respArtista = await _cRA.Guardar(artista);
                }

                oUsuario.Id = respuesta.objecto.Id;

                HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(oUsuario));
                return RedirectToAction("Index", "Home");
            }

            Respuesta<TipoUsuario> respuestaTipoUsuario = await _cRTU.ObtenerLista();
            ViewData["TipoUsuario"] = new SelectList(respuestaTipoUsuario.lista, "Id", "Descripcion", oUsuario.TipoUsuario);

            return View(oUsuario);
        }
    }
}
