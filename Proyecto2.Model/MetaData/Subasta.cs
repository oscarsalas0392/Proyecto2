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
    [ModelMetadataType(typeof(SubastaMetadata))]
    public partial class Subasta
    {
        public class SubastaMetadata
        {
            [Required(ErrorMessage = "El precio inicial es requerido.")]
            public decimal? PrecioInicial { get; set; }

            [Required(ErrorMessage = "La fecha inicial es requerida.")]
            public DateTime FechaInicial { get; set; }

            [Required(ErrorMessage = "El fecha de ciere es requerida.")]
            public DateTime FechaCierre { get; set; }
        }
    }
}
