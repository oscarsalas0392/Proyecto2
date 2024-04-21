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
        public bool? fechaInicial { get; set; } = null;
        public bool? fechaCierre { get; set; } = null;

        public Dictionary<string, string> columnas { get; set; } = new Dictionary<string, string>();


        public Filtro()
        {
            numeroPagina = 1;
            tamanoPagina = 10;
            columnaOrdenar = "Id";
            tipoOrdernar = "ASC";
            columnas.Add("Usuario", "Usuario");
            columnas.Add("Artista", "Artista");
            columnas.Add("ObraArte", "ObraArte");
            columnas.Add("FechaInicial", "FechaInicial");
            columnas.Add("FechaCierre", "FechaCierre");
        }
    }

}
