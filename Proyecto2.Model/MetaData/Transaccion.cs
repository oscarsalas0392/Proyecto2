using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Proyecto2.Model
{
    [ModelMetadataType(typeof(TransaccionMetadata))]
    public partial class Transaccion
    {
        public class TransaccionMetadata
        {
            [Required(ErrorMessage = "La tarjeta es requerida.")]
            public string Tarjeta { get; set; }
        }
    }
}
