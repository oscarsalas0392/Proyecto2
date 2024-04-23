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
    public class NotificacionRepositorio : BaseRepositorio<Notificacion, int?>
    {
        public NotificacionRepositorio(Context db) : base(db)
        {

        }

        public override Task<Respuesta<Notificacion>> Guardar(Notificacion model)
        {
            model.PorApp = true;
            model.Fecha = DateTime.Now;
            return base.Guardar(model);
        }
        public async Task<Respuesta<Notificacion>> EnviarOfertaUsuarios(Subasta subasta, int idUsuario)
        {

            try
            {
                Notificacion notificacion = new Notificacion();
                List<Usuario> usuarios = _db.Oferta
                                         .Where(x => x.Subasta == subasta.Id && x.Usuario != idUsuario)
                                         .GroupBy(x => x.Usuario)
                                         .Select(x => new Usuario
                                         {
                                             Id = x.Key
                                         })
                                         .ToList();


                foreach (var usuario in usuarios)
                {
                    notificacion = new Notificacion();
                    notificacion.Usuario = usuario.Id;
                    notificacion.Titulo = Mensajes.OfertaSubasta.titulo;
                    notificacion.Descripcion = Mensajes.OfertaSubasta.Descripcion + subasta.ObraArteNavigation.Titulo;
                    await Guardar(notificacion);
                }

                notificacion = new Notificacion();
                notificacion.Usuario = idUsuario;
                notificacion.Titulo = Mensajes.Oferta.titulo;
                notificacion.Descripcion = Mensajes.Oferta.Descripcion + subasta.ObraArteNavigation.Titulo;
                await Guardar(notificacion);

                return new Respuesta<Notificacion>(true, Accion.agregar);
            }
            catch (Exception)
            {
                return new Respuesta<Notificacion>(true, Accion.agregar, true);
            }

        }
    }
    
}
