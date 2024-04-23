using Microsoft.AspNetCore.Mvc;
using Proyecto2.ClasesBase;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Data.Interfaces;
using Proyecto2.Extensions;
using Proyecto2.Model;
using Proyecto2.ViewModels;

namespace Proyecto2.Controllers
{
    public class HistorialController : BaseController
    {
        private readonly Context _context;
        private readonly IRepositorio<Subasta, int?> _cR;
        private readonly SubastaRepositorio _cR2;

        public HistorialController(Context context, IRepositorio<Subasta, int?> cR, SubastaRepositorio cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2; 
        }

        public async Task<IActionResult> Index(IndexViewModel<Subasta, SubastaRepositorio, int?> vm)
        {
            vm.Command = "historial";
            if (Artista()?.Id == null || Artista()?.Id == 0)
            {
                await vm.HandleRequest(_cR2, "ObraArteNavigation.Titulo", "ObraArteNavigation.Titulo", Usuario().Id);
            }
            else 
            {
                await vm.HandleRequest(_cR2, "ObraArteNavigation.Titulo", "ObraArteNavigation.Titulo", artista: Artista().Id);
            }
        

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        
        }

    }
}
