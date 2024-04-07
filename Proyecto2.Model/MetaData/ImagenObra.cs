using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Model
{
    [ModelMetadataType(typeof(ImagenObraMetadata))]
    public partial class ImagenObra
    {
        public class ImagenObraMetadata
        {
            [Required(ErrorMessage = "La imagen es requerida.")]
            public string Imagen { get; set; }
        }
    }
}
