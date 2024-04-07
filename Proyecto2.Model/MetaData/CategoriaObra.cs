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
    [ModelMetadataType(typeof(CategoriaObraMetadata))]
    public partial class CategoriaObra
    {
        public class CategoriaObraMetadata
        {
            [Required(ErrorMessage = "La descripción es requerida.")]
            public string Descripcion { get; set; }
        }
    }
}
