using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Model
{
    [ModelMetadataType(typeof(DimensionObraMetadata))]
    public partial class DimensionObra
    {
        public class DimensionObraMetadata
        {
            [Required(ErrorMessage = "La altura es requerida.")]
            public decimal Altura { get; set; }

            [Required(ErrorMessage = "El ancho es requerido.")]
            public decimal Ancho { get; set; }

            [Required(ErrorMessage = "La profundidad es requerida.")]
            public decimal Profundidad { get; set; }
        }
    }
}
