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
    [ModelMetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario
    {
        public class UsuarioMetadata
        {
            [Required(ErrorMessage = "La nombre de usuario es requerido.")]
            public string NombreUsuario { get; set; }

            [Required(ErrorMessage = "El correo es requerido.")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida.")]
            public string Contrasena { get; set; }

            [Required(ErrorMessage = "La tipo de usuario es requerido.")]
            public int TipoUsuario { get; set; }
        }
    }
}
