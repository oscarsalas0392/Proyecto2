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
    [ModelMetadataType(typeof(OfertaMetadata))]
    public partial class Oferta
    {
        public class OfertaMetadata
        {
            [Required(ErrorMessage = "El monto es requerido.")]
            public decimal Monto { get; set; }
        }
    }
}
