using Microsoft.EntityFrameworkCore;
using Proyecto2.Data.Clases;
using Proyecto2.Data.ClasesBase;
using Proyecto2.Model;
using Proyecto2.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data.ClasesRepository
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario, int?>
    {
        public UsuarioRepositorio(Context db) : base(db)
        {
            lstIncludes.Add("TipoUsuarioNavigation");  
        }

        public async Task<Respuesta<Usuario>> ValidarUsuario(string correo, string contrasena)
        {
            try
            {
                contrasena = DecryptAndEncrypt.EncryptStringAES(contrasena);
                if (_db is null) { return new Respuesta<Usuario>(true, Accion.obtenerLista, true); }
                Usuario? usuario = await _db.Usuario.Where(x => (x.Correo == correo || x.NombreUsuario == correo) && x.Contrasena == contrasena).FirstOrDefaultAsync();
                Respuesta<Usuario> notificacion = new Respuesta<Usuario>(true, Accion.obtener);
                notificacion.objecto = usuario;
                return notificacion;
            }
            catch
            {
                Respuesta<Usuario> notificacion = new Respuesta<Usuario>(true, Accion.obtener, true);
                notificacion.objecto = null;
                return notificacion;
            }

        }

        public override async Task<Respuesta<Usuario>> Guardar(Usuario model)
        {
            Usuario usuario = new Usuario()
            {
                NombreUsuario = model.NombreUsuario,
                Correo = model.Correo, 
                Contrasena = DecryptAndEncrypt.EncryptStringAES(model.Contrasena),
                TipoUsuario = model.TipoUsuario,
                Foto = null

            };

            List<Usuario> validaciones = _db.Usuario.Where(x => x.Correo == model.Correo || x.NombreUsuario == model.NombreUsuario).ToList();
            Respuesta<Usuario> notificacion = new Respuesta<Usuario>(false, Accion.agregar);

            if (validaciones.Exists(x => x.Correo == model.Correo))
            {
                notificacion.mensaje = Mensajes.EXISTS_EMAIL;
                return notificacion;
            }
            if (validaciones.Exists(x => x.NombreUsuario == model.NombreUsuario))
            {
                notificacion.mensaje = Mensajes.EXISTS_USERNAME;
                return notificacion;
            }

            return await base.Guardar(usuario);
        }
    }
}
