using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Extensions;
using Proyecto2.Model;
using Proyecto2.Models;
using Proyecto2.Respuesta;
using Proyecto2.ViewModels;
using System.Diagnostics;

namespace Proyecto2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;
        private readonly SubastaRepositorio _cR;
        private readonly OfertaRepositorio _cROF;
        private readonly NotificacionRepositorio _cRN;
        public HomeController(ILogger<HomeController> logger, Context context, SubastaRepositorio cR, OfertaRepositorio cROF, NotificacionRepositorio cRN)
        {
            _logger = logger;
            _context = context;
            _cR = cR;
            _cROF = cROF;
            _cRN = cRN;
        }

        public async Task<IActionResult> Index(IndexViewModel<Subasta, SubastaRepositorio, int?> vm)
        {
            vm.paginacion.fechaInicial = true;
            vm.paginacion.fechaCierre = true;
            await vm.HandleRequest(_cR, "ObraArteNavigation.Titulo", "ObraArteNavigation.Titulo");

            string? json = HttpContext.Session.GetString("usuario");

            if (json != null && json != "")
            {
                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);

                Filtro filtro = new Filtro();
                filtro.usuario = usuario.Id;
                Respuesta<Notificacion> respuesta = await _cRN.ObtenerLista(filtro);
                ViewBag.notificaciones = respuesta.lista;

                Respuesta<Oferta> respOferta =  _cROF.VerificarGanador(usuario.Id);
                ViewBag.ganador = respOferta.objecto;
            }

    

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }

            return View(vm);
        }

        public async Task<IActionResult> Subasta(int id)
        {
            Respuesta<Subasta> respuesta =  await _cR.ObtenerId(id);
       
            return View(respuesta.objecto);
        }

        public  async Task<IActionResult> Ofertar([Bind("Id,ObraArte,PrecioInicial,PrecioActual,FechaInicial,FechaCierre,Eliminado")] Subasta subasta)
        {

            Respuesta<Subasta> respuestaSubasta = await _cR.ObtenerId(subasta.Id);
            string? json = HttpContext.Session.GetString("usuario");

            if (json == null || json == "")
            {
                return RedirectToAction("Login", "Acceso");
            }

            Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);

            if (usuario == null || subasta.PrecioActual == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            Oferta oferta =  new Oferta();
            oferta.Subasta = subasta.Id;
            oferta.Fecha = DateTime.Now;
            oferta.Usuario = usuario.Id;
            oferta.Monto = subasta.PrecioActual.Value;

            Respuesta<Oferta> respuesta = await _cROF.Guardar(oferta);

            await _cRN.EnviarOfertaUsuarios(respuestaSubasta.objecto, usuario.Id);


            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
