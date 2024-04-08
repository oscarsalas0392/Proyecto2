using Microsoft.EntityFrameworkCore;
using Proyecto2.Data.Interfaces;
using Proyecto2.Respuesta;
using Proyecto2.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Proyecto2.Data.ClasesBase
{
    public abstract class BaseRepositorio<T, TK> : IRepositorio<T, TK> where T : class
    {
        public List<string> lstIncludes = new List<string>();

        readonly protected Context? _db = null;
        public string _columnaPK { get; set; } = "Id";

        public BaseRepositorio(Context db)
        {
            _db = db;
        }

        public virtual async Task<Respuesta<T>> Guardar(T model)
        {
            try
            {
                if (_db is null) { return new Respuesta<T>(true, Accion.agregar, true); }

                PropertyInfo propertyInfo = model.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(model, false);

                await _db.Set<T>().AddAsync(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Respuesta<T> Respuesta = new Respuesta<T>(blnResultado, Accion.agregar);
                Respuesta.objecto = model;
                return Respuesta;

            }
            catch (Exception)
            {
                return new Respuesta<T>(true, Accion.agregar, true);
            }
        }
        public virtual async Task<Respuesta<T>> Actualizar(T model)
        {
            try
            {
                if (_db is null) { return new Respuesta<T>(true, Accion.actualizar, true); }

                PropertyInfo propertyInfo = model.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(model, false);

                _db.Set<T>().Update(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Respuesta<T> Respuesta = new Respuesta<T>(blnResultado, Accion.actualizar);
                return Respuesta;

            }
            catch (Exception)
            {
                return new Respuesta<T>(true, Accion.actualizar, true);
            }
        }
        public virtual async Task<Respuesta<T>> ObtenerId(TK? key)
        {

            try
            {
                if (_db is null || key is null) { return new Respuesta<T>(false, Accion.obtener); }

                IQueryable<T> query = _db.Set<T>();

                //Configuracion de includes
                foreach (string include in lstIncludes)
                {
                    query = query.Include(include);
                }

                T? objecto = await query.Where($"{_columnaPK} = {key}").FirstOrDefaultAsync();
                Respuesta<T> Respuesta = new Respuesta<T>(objecto is not null, Accion.obtener);
                Respuesta.objecto = objecto;
                return Respuesta;

            }
            catch (Exception)
            {
                return new Respuesta<T>(true, Accion.obtener, true);
            }

        }
        public void Dispose()
        {

        }

        public virtual async Task<Respuesta<T>> ObtenerLista(Filtro? pFiltro = null)
        {
            try
            {
                var result = new List<T>();
                if (_db is null) { return new Respuesta<T>(true, Accion.obtener, true); }

                if (pFiltro == null)
                {
                    result = await _db.Set<T>().Where($"Eliminado = false").AsNoTracking().ToListAsync();
                }
                else
                {
                    IQueryable<T> query = _db.Set<T>();

                    //Configuracion de includes
                    foreach (string include in lstIncludes)
                    {
                        query = query.Include(include);
                    }

                    //Configuracion de where
                    query = query
                        .Where($"Eliminado = false " +
                           $"{(pFiltro.usuario != null ? $" && Usuario = {pFiltro.usuario}" : "")} " +
                           $"{(pFiltro.artista != null ? $" && Artista = {pFiltro.artista}" : "")}" +
                           $")");

                    //Se obtiene la cantidad registros de la tabla
                    pFiltro.cantidadRegistros = await query.CountAsync();

                    //Se realiza la paginacion
                    result = await query.
                        OrderBy(pFiltro.Ordenando).
                        Skip((pFiltro.numeroPagina - 1) * pFiltro.tamanoPagina).
                        Take(pFiltro.tamanoPagina).ToListAsync();
                }

                Respuesta<T> Respuesta = new Respuesta<T>(result is not null, Accion.obtenerLista);
                Respuesta.lista = result;
                return Respuesta;
            }
            catch (Exception)
            {
                Respuesta<T> Respuesta = new Respuesta<T>(true, Accion.obtener, true);
                Respuesta.lista = new List<T>();
                return Respuesta;
            }
        }
        public virtual async Task<Respuesta<T>> Eliminar(TK key)
        {
            try
            {
                if (_db is null) { return new Respuesta<T>(true, Accion.obtener, true); }
                Respuesta<T> Respuesta = await ObtenerId(key);
                if (!Respuesta._estado || Respuesta._excepcion || Respuesta.objecto is null)
                {
                    return Respuesta;
                }

                T? objecto = Respuesta.objecto;
                PropertyInfo propertyInfo = objecto.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(objecto, true);

                _db.Set<T>().Update(objecto);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Respuesta<T> RespuestaActualizar = new Respuesta<T>(blnResultado, Accion.eliminar);
                return RespuestaActualizar;
            }
            catch (Exception)
            {
                return new Respuesta<T>(true, Accion.eliminar, true);
            }

        }
        public virtual async Task<Respuesta<T>> Buscar(string filtro, Filtro pFiltro)
        {
            try
            {
                filtro = filtro == null ? "" : filtro;
                var result = new List<T>();

                if (_db is null) { return new Respuesta<T>(true, Accion.obtenerLista, true); }

                IQueryable<T> query = _db.Set<T>();

                //Configuracion de includes
                foreach (string include in lstIncludes)
                {
                    query = query.Include(include);
                }

                //Configuracion de where
                query = query
                    .Where($"Eliminado = false " +
                           $"{(pFiltro.usuario != null ? $" && Usuario = {pFiltro.usuario}" : "")} " +
                           $"{(pFiltro.artista != null ? $" && Artista = {pFiltro.artista}" : "")}" +
                           $"&& {pFiltro.columnaBuscar}.Contains(\"{filtro}\")"); ;
                pFiltro.cantidadRegistros = await query.CountAsync();

                result = await query.
                    OrderBy(pFiltro.Ordenando).
                    Skip((pFiltro.numeroPagina - 1) * pFiltro.tamanoPagina).
                    Take(pFiltro.tamanoPagina).ToListAsync();

                Respuesta<T> Respuesta = new Respuesta<T>(result is not null, Accion.obtenerLista);
                Respuesta.lista = result;
                return Respuesta;
            }
            catch (Exception)
            {
                Respuesta<T> Respuesta = new Respuesta<T>(true, Accion.obtener, true);
                Respuesta.lista = new List<T>();
                return Respuesta;
            }
        }
    }
}