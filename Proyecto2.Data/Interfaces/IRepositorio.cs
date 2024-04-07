using Proyecto2.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data.Interfaces
{
    public interface IRepositorio<T, TK> : IDisposable
    {
        Task<Notificacion<T>> Guardar(T model);
        Task<Notificacion<T>> Actualizar(T model);
        Task<Notificacion<T>> ObtenerId(TK? key);
        Task<Notificacion<T>> ObtenerLista(Filtro? pf = null);
        Task<Notificacion<T>> Eliminar(TK key);
        Task<Notificacion<T>> Buscar(string filtro, Filtro pf);
    }
}
