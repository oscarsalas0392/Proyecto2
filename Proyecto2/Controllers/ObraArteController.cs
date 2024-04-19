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
    public class ObraArteController : BaseController
    {
        private readonly Context _context;
        private readonly IRepositorio<ObraArte, int?> _cR;
        private readonly ObraArteRepositorio _cR2;
        private readonly CategoriaObraRepositorio _cRCO;
        private readonly DimensionObraRepositorio _cRDO;
        private readonly ImagenObraRepositorio _cRIO;
        public ObraArteController(Context context, IRepositorio<ObraArte, int?> cR, ObraArteRepositorio cR2, CategoriaObraRepositorio cRCO, DimensionObraRepositorio cRDO, ImagenObraRepositorio cRIO)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
            _cRCO = cRCO;
            _cRDO = cRDO;
            _cRIO = cRIO;
        }

        // GET: ObraArte
        public async Task<IActionResult> Index(IndexViewModel<ObraArte, ObraArteRepositorio, int?> vm)
        {
            await vm.HandleRequest(_cR2, "Titulo", "Titulo", artista:Artista().Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexTable", vm);
            }
            return View(vm);
        }

        // GET: ObraArte/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Respuesta<ObraArte> respuesta = await _cR.ObtenerId(id);

            if (!respuesta._estado || respuesta._excepcion)
            {
                return NotFound();
            }

            return View(respuesta.objecto);
        }

        // GET: ObraArte/Create
        public async  Task<IActionResult> Create(int? id)
        {
            Respuesta<CategoriaObra> respuesta = await _cRCO.ObtenerLista();
            ViewData["CategoriaObra"] = new SelectList(respuesta.lista, "Id", "Descripcion");

            if (id != null)
            {
                Respuesta<ObraArte> respuestaObraArte = await _cR.ObtenerId(id);

                if (!respuestaObraArte._estado || respuestaObraArte._excepcion || respuestaObraArte.objecto is null)
                {
                    return NotFound();
                }

                Respuesta<DimensionObra> respuestaDimensiones = await _cRDO.ObtenerId(id);

                Filtro filtro = new Filtro();
                filtro.obraArte = respuestaObraArte.objecto.Id;
                Respuesta<ImagenObra> respuestaImagen = await _cRIO.ObtenerLista(filtro);

                ObraArteViewModel obraArteViewModel = new ObraArteViewModel();

                obraArteViewModel.obraArte =respuestaObraArte.objecto;
                obraArteViewModel.dimensionObra = respuestaDimensiones.objecto;
                obraArteViewModel.listImagenesObra = respuestaImagen.lista.ToList();

                return View(obraArteViewModel);
            }

            return View();
        }

        // POST: ObraArte/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "obraArte")] ObraArte obraArte, [Bind(Prefix = "dimensionObra")] DimensionObra dimensionObra, [Bind(Prefix = "listImgAgregar")] string listImgAgregar, [Bind(Prefix = "listImgEliminar")] string listImgEliminar)
        {
            ObraArteViewModel obraArteViewModel = new ObraArteViewModel();
            obraArte.Artista = Artista().Id;
            obraArteViewModel.obraArte = obraArte;
            obraArteViewModel.dimensionObra = dimensionObra;

            ModelState.Remove("listImgAgregar");
            ModelState.Remove("listImgEliminar");
            if (ModelState.IsValid)
            {
                Respuesta<ObraArte> respObraArte = await _cR.Guardar(obraArte);

                if (!respObraArte._estado || respObraArte._excepcion || respObraArte.objecto is null)
                {
                    return View(obraArteViewModel);
                }

                dimensionObra.ObraArte = respObraArte.objecto.Id;
                Respuesta<DimensionObra> respDimensionObra = await _cRDO.Guardar(dimensionObra);
                List<string> agregar = listImgAgregar.Split('^').ToList(); 
                List<string> eliminar = listImgEliminar == null ? new List<string>() :listImgEliminar.Split('^').ToList();

                List<string> elementos = agregar.Where(x => eliminar.Contains(x) == false && x != "").ToList();
                    
                foreach(string imagen in elementos)
                {
                    ImagenObra imagenObra = new ImagenObra();
                    imagenObra.Imagen = imagen;
                    imagenObra.ObraArte = respObraArte.objecto.Id;
                    Respuesta<ImagenObra> respImagenObra = await _cRIO.Guardar(imagenObra);
                }  
                return RedirectToAction(nameof(Index));
            }
         

            Respuesta<CategoriaObra> respuesta = await _cRCO.ObtenerLista();
            ViewData["CategoriaObra"] = new SelectList(respuesta.lista, "Id", "Descripcion");
            return View(obraArteViewModel);
        }

        // GET: ObraArte/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Respuesta<ObraArte> respuestaObraArte = await _cR.ObtenerId(id);

            if (!respuestaObraArte._estado || respuestaObraArte._excepcion || respuestaObraArte.objecto is null)
            {
                return NotFound();
            }


            Respuesta<CategoriaObra> respuesta = await _cRCO.ObtenerLista();
            ViewData["CategoriaObra"] = new SelectList(respuesta.lista, "Id", "Descripcion");

            return View(respuestaObraArte.objecto);
        }

        // POST: ObraArte/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Artista,CategoriaObra,Titulo,Descripcion,Eliminado")] ObraArte obraArte)
        {
            if (id != obraArte.Id)
            {
                return NotFound();
            }
            obraArte.Artista = Artista().Id;
            if (ModelState.IsValid)
            {
                Respuesta<ObraArte> respuestaObraArte = await _cR.Actualizar(obraArte);

                if (!respuestaObraArte._estado || respuestaObraArte._excepcion)
                {
                    return View(obraArte);
                }
                return RedirectToAction(nameof(Index));


            }
            Respuesta<CategoriaObra> respuesta = await _cRCO.ObtenerLista();
            ViewData["CategoriaObra"] = new SelectList(respuesta.lista, "Id", "Descripcion");

            return View(obraArte);
        }

        // GET: ObraArte/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Respuesta<ObraArte> respuestaObraArte = await _cR.ObtenerId(id);

            if (!respuestaObraArte._estado || respuestaObraArte._excepcion)
            {
                return NotFound();
            }
            return View(respuestaObraArte.objecto);
        }

        // POST: ObraArte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Respuesta<ObraArte> respuestaObraArte = await _cR.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

   
    }
}
