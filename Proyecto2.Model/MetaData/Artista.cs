using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Model
{
    [ModelMetadataType(typeof(ArtistaMetadata))]
    public partial class Artista
    {
        public class ArtistaMetadata
        {
            [Required(ErrorMessage = "El nombre es requerido.")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La información es requerida.")]
            public string Informacion { get; set; }

            [Required(ErrorMessage = "El estilo es requerido.")]
            public string Estilo { get; set; }

            [Required(ErrorMessage = "La experiencia es requerida.")]
            public string Experiencia { get; set; }

            [Required(ErrorMessage = "El enlace es requerido.")]
            public string Enlace { get; set; }
        }
    }
}
