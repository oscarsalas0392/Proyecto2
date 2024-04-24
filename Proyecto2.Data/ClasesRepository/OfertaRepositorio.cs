using Microsoft.EntityFrameworkCore;
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
    public class OfertaRepositorio : BaseRepositorio<Oferta, int?>
    {
        public OfertaRepositorio(Context db) : base(db)
        {
            lstIncludes.Add("SubastaNavigation");
            lstIncludes.Add("UsuarioNavigation");
        }

        public Respuesta<Oferta> VerificarGanador(int idUsuario)
        {
            try
            {
                Subasta? subasta = _db.Subasta.Include("Oferta").Include("Transaccion")
                .Where(x => x.FechaCierre < DateTime.Now && x.Oferta.Count > 0).FirstOrDefault();

                Oferta? oferta = subasta?.Oferta?.LastOrDefault();
                Transaccion? transaccion = subasta?.Transaccion?.FirstOrDefault();

                if (subasta != null && oferta.Usuario == idUsuario && transaccion == null)
                {
                    Respuesta<Oferta> respuesta = new Respuesta<Oferta>(true, Accion.obtener);
                    respuesta.objecto = oferta;
                    return respuesta;
                }

                return new Respuesta<Oferta>(false, Accion.obtener);
            }
            catch (Exception)
            {
                return new Respuesta<Oferta>(true, Accion.obtener,true);
            }
  
        }
    }
}
