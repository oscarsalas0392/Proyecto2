using Proyecto2.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data.Interfaces
{
    public interface IRepositorio<T, TK> : IDisposable
    {
        Task<Respuesta<T>> Guardar(T model);
        Task<Respuesta<T>> Actualizar(T model);
        Task<Respuesta<T>> ObtenerId(TK? key);
        Task<Respuesta<T>> ObtenerLista(Filtro? pf = null);
        Task<Respuesta<T>> Eliminar(TK key);
        Task<Respuesta<T>> Buscar(string filtro, Filtro pf);
    }
}
