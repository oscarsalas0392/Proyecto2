using Microsoft.EntityFrameworkCore;
using Proyecto2.Data.Interfaces;
using Proyecto2.Notificacion;
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

        public virtual async Task<Notificacion<T>> Guardar(T model)
        {
            try
            {
                if (_db is null) { return new Notificacion<T>(true, Accion.agregar, true); }

                PropertyInfo propertyInfo = model.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(model, false);

                await _db.Set<T>().AddAsync(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Notificacion<T> notificacion = new Notificacion<T>(blnResultado, Accion.agregar);
                notificacion.objecto = model;
                return notificacion;

            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.agregar, true);
            }
        }
        public virtual async Task<Notificacion<T>> Actualizar(T model)
        {
            try
            {
                if (_db is null) { return new Notificacion<T>(true, Accion.actualizar, true); }

                PropertyInfo propertyInfo = model.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(model, false);

                _db.Set<T>().Update(model);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Notificacion<T> notificacion = new Notificacion<T>(blnResultado, Accion.actualizar);
                return notificacion;

            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.actualizar, true);
            }
        }
        public virtual async Task<Notificacion<T>> ObtenerId(TK? key)
        {

            try
            {
                if (_db is null || key is null) { return new Notificacion<T>(false, Accion.obtener); }

                IQueryable<T> query = _db.Set<T>();

                //Configuracion de includes
                foreach (string include in lstIncludes)
                {
                    query = query.Include(include);
                }

                T? objecto = await query.Where($"{_columnaPK} = {key}").FirstOrDefaultAsync();
                Notificacion<T> notificacion = new Notificacion<T>(objecto is not null, Accion.obtener);
                notificacion.objecto = objecto;
                return notificacion;

            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.obtener, true);
            }

        }
        public void Dispose()
        {

        }

        public virtual async Task<Notificacion<T>> ObtenerLista(Filtro? pFiltro = null)
        {
            try
            {
                var result = new List<T>();
                if (_db is null) { return new Notificacion<T>(true, Accion.obtener, true); }

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

                Notificacion<T> notificacion = new Notificacion<T>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<T> notificacion = new Notificacion<T>(true, Accion.obtener, true);
                notificacion.lista = new List<T>();
                return notificacion;
            }
        }
        public virtual async Task<Notificacion<T>> Eliminar(TK key)
        {
            try
            {
                if (_db is null) { return new Notificacion<T>(true, Accion.obtener, true); }
                Notificacion<T> notificacion = await ObtenerId(key);
                if (!notificacion._estado || notificacion._excepcion || notificacion.objecto is null)
                {
                    return notificacion;
                }

                T? objecto = notificacion.objecto;
                PropertyInfo propertyInfo = objecto.GetType().GetProperty("Eliminado");
                propertyInfo.SetValue(objecto, true);

                _db.Set<T>().Update(objecto);
                int resultado = await _db.SaveChangesAsync();
                bool blnResultado = resultado == 1 ? true : false;

                Notificacion<T> notificacionActualizar = new Notificacion<T>(blnResultado, Accion.eliminar);
                return notificacionActualizar;
            }
            catch (Exception)
            {
                return new Notificacion<T>(true, Accion.eliminar, true);
            }

        }
        public virtual async Task<Notificacion<T>> Buscar(string filtro, Filtro pFiltro)
        {
            try
            {
                filtro = filtro == null ? "" : filtro;
                var result = new List<T>();

                if (_db is null) { return new Notificacion<T>(true, Accion.obtenerLista, true); }

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

                Notificacion<T> notificacion = new Notificacion<T>(result is not null, Accion.obtenerLista);
                notificacion.lista = result;
                return notificacion;
            }
            catch (Exception)
            {
                Notificacion<T> notificacion = new Notificacion<T>(true, Accion.obtener, true);
                notificacion.lista = new List<T>();
                return notificacion;
            }
        }
    }
}