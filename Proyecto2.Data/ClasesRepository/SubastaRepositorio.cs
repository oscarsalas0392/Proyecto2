using Microsoft.EntityFrameworkCore;
using Proyecto2.Data.ClasesBase;
using Proyecto2.Model;
using Proyecto2.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Proyecto2.Data.ClasesRepository
{
    public class SubastaRepositorio : BaseRepositorio<Subasta, int?>
    {
        public SubastaRepositorio(Context db) : base(db)
        {
            lstIncludes.Add("ObraArteNavigation");
            lstIncludes.Add("ObraArteNavigation.ImagenObra");
            lstIncludes.Add("ObraArteNavigation.DimensionObra");
            lstIncludes.Add("ObraArteNavigation.CategoriaObraNavigation");
            lstIncludes.Add("ObraArteNavigation.ArtistaNavigation");
            lstIncludes.Add("Oferta");         
        }

        public  Respuesta<bool> ExisteSubasta(int idObra)
        {
            try
            {
                var result = _db.Subasta.Where(x => x.ObraArte == idObra).FirstOrDefault();
                bool existe = result == null ? false : true;
                Respuesta<bool> Respuesta = new Respuesta<bool>(true, Accion.obtener);
                Respuesta.objecto = existe;
                return Respuesta;
            }
            catch (Exception ex) {
                return new Respuesta<bool>(true, Accion.obtener, true);

            }



        }

        public async Task<Respuesta<Subasta>> HistorialUsuarios(Filtro filtro)
        {
            try
            {
                  IQueryable<Oferta> query = _db.Oferta;

                  query = query
                                .Include(x => x.SubastaNavigation)
                                .Include(x => x.SubastaNavigation.ObraArteNavigation)
                                .Where(x => x.Usuario == filtro.usuario);

                filtro.cantidadRegistros = await query.CountAsync();

                List<Subasta> subastas = await query
                                                 .Select(x => x.SubastaNavigation)
                                                 .OrderBy(filtro.Ordenando).
                                                  Skip((filtro.numeroPagina - 1) * filtro.tamanoPagina).
                                                  Take(filtro.tamanoPagina)
                                                 .ToListAsync();

                 Respuesta<Subasta> respuesta = new Respuesta<Subasta>(true, Accion.obtenerLista);
                 respuesta.lista = subastas;
                 return respuesta;
            }
            catch 
            {
                Respuesta<Subasta> respuesta = new Respuesta<Subasta>(true, Accion.obtener, true);
                respuesta.lista = new List<Subasta>();
                return respuesta;
            }

        }

        public async Task<Respuesta<Subasta>> HistorialArtistas(Filtro filtro)
        {
            try
            {
                IQueryable<Oferta> query = _db.Oferta;

                query = query
                       .Include(x => x.SubastaNavigation)
                       .Include(x => x.SubastaNavigation.ObraArteNavigation)
                       .Where(x => x.SubastaNavigation.ObraArteNavigation.Artista == filtro.artista);

                filtro.cantidadRegistros = await query.CountAsync();

                List<Subasta> subastas = await query
                                               .Select(x => x.SubastaNavigation)
                                               .OrderBy("Titulo").
                                                Skip((filtro.numeroPagina - 1) * filtro.tamanoPagina).
                                                Take(filtro.tamanoPagina)
                                               .ToListAsync();

                Respuesta<Subasta> respuesta = new Respuesta<Subasta>(true, Accion.obtenerLista);
                respuesta.lista = subastas;
                return respuesta;
            }
            catch
            {
                Respuesta<Subasta> respuesta = new Respuesta<Subasta>(true, Accion.obtener, true);
                respuesta.lista = new List<Subasta>();
                return respuesta;
            }

        }

        public override async Task<Respuesta<Subasta>> Historial(Filtro? pf = null)
        {
            if (pf.artista == null)
            {
                return await HistorialUsuarios(pf);
            }
            else 
            {
                return await HistorialArtistas(pf);
            }   
        }

    }
}
