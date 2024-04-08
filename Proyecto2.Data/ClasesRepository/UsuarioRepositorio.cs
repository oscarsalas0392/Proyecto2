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

    }
}
