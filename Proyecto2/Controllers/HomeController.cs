using Microsoft.AspNetCore.Mvc;
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
        public HomeController(ILogger<HomeController> logger, Context context, SubastaRepositorio cR)
        {
            _logger = logger;
            _context = context;
            _cR = cR;
        }

        public async Task<IActionResult> Index(IndexViewModel<Subasta, SubastaRepositorio, int?> vm)
        {
            vm.paginacion.fechaInicial = true;
            vm.paginacion.fechaCierre = true;
            await vm.HandleRequest(_cR, "ObraArteNavigation.Titulo", "ObraArteNavigation.Titulo");
           
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
