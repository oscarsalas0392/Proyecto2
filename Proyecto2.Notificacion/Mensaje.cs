using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto2.Respuesta
{
    public class Mensaje
    {
        public Mensaje()
        {
            Params = new List<string>();
        }
        public Mensaje(int id, Nivel level, string description) : this()
        {
            this.id = id;
            this.nivel = level;
            this.Descripcion = description;
        }
        public int id { get; set; }
        public Nivel nivel { get; set; } = Nivel.Exception;
        public string titulo { get; set; } = "";

        string _descripcion = "";

        public string Descripcion
        {
            get
            {
                if (Params.Any())
                {
                    return string.Format(_descripcion, Params.ToArray());
                }

                return _descripcion;
            }
            set
            {
                _descripcion = value;
            }
        }


        [JsonIgnore]
        public List<string> Params { get; set; }

    }
}
