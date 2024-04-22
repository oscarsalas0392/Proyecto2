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
            return base.Guardar(model);
        }
    }
}
