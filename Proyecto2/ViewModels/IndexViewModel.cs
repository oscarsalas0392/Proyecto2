using Proyecto2.Data;
using Proyecto2.Data.ClasesBase;
using Proyecto2.Model;
using Proyecto2.Respuesta;

namespace Proyecto2.ViewModels
{
    public class IndexViewModel<T, TR, TK> where T : class where TR : BaseRepositorio<T, TK>
    {
        TR? _cR;
        public string Command { get; set; } = "list";
        public CategoriaObra filtro { get; set; } = new CategoriaObra();
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public Filtro paginacion { get; set; } = new Filtro() { columnaOrdenar = "Descripcion", columnaBuscar = "Descripcion" };


        public async Task HandleRequest(TR cR, string columnaOrdenar = "Descripcion", string columnaBuscar = "Descripcion", int? usuario = null)
        {
            paginacion.columnaBuscar = columnaBuscar;
            paginacion.columnaOrdenar = columnaOrdenar;
            paginacion.usuario = usuario;
            _cR = cR;
            await EjecutarComando(Command);
        }

        private async Task EjecutarComando(string comando)
        {
            switch (comando)
            {
                case "list":
                    await list();
                    break;
                case "search":
                case "paging":
                case "order":
                    await search();
                    break;
                default:
                    break;
            }

        }

        async Task list()
        {

            Respuesta<T> notificacion = await _cR.ObtenerLista(paginacion);
            if (notificacion.lista != null)
            {
                Items = notificacion.lista;
            }
        }

        async Task search()
        {
            Respuesta<T> notificacion = await _cR.Buscar(filtro.Descripcion, paginacion);
            if (notificacion.lista != null)
            {
                Items = notificacion.lista;
            }
        }
    }
}
