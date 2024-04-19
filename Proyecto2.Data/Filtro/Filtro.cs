using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data
{
    public class Filtro
    {
        public int numeroPagina { get; set; }
        public int tamanoPagina { get; set; }
        public int elementosPagina => (int)Math.Ceiling((decimal)cantidadRegistros / tamanoPagina);
        public string columnaOrdenar { get; set; } = "Id";
        public string columnaBuscar { get; set; } = "Id";
        public string tipoOrdernar { get; set; } = "ASC";
        public string Ordenando
        {
            get
            {
                if (string.IsNullOrEmpty(columnaOrdenar))
                {
                    throw new InvalidOperationException("The PageFilter needs a default sort.");
                }

                return string.IsNullOrEmpty(columnaOrdenar) ? "" : $"{columnaOrdenar} {tipoOrdernar}";
            }
        }
        public int cantidadRegistros { get; set; }

        public int? usuario { get; set; } = null;
        public int? artista { get; set; } = null;

        public int? obraArte { get; set; } = null;
        public Filtro()
        {
            numeroPagina = 1;
            tamanoPagina = 10;
            columnaOrdenar = "Id";
            tipoOrdernar = "ASC";
        }
    }
}
