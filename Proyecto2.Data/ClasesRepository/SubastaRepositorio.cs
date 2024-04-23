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
    public class SubastaRepositorio : BaseRepositorio<Subasta, int?>
    {
        public SubastaRepositorio(Context db) : base(db)
        {
            lstIncludes.Add("ObraArteNavigation");
            lstIncludes.Add("ObraArteNavigation.ImagenObra");
            lstIncludes.Add("ObraArteNavigation.DimensionObra");
            lstIncludes.Add("ObraArteNavigation.CategoriaObraNavigation");
            lstIncludes.Add("ObraArteNavigation.ArtistaNavigation");
            lstIncludes.Add("Oferta");         
        }

        public  Respuesta<bool> ExisteSubasta(int idObra)
        {
            try
            {
                var result = _db.Subasta.Where(x => x.ObraArte == idObra).FirstOrDefault();
                bool existe = result == null ? false : true;
                Respuesta<bool> Respuesta = new Respuesta<bool>(true, Accion.obtener);
                Respuesta.objecto = existe;
                return Respuesta;
            }
            catch (Exception ex) {
                return new Respuesta<bool>(true, Accion.obtener, true);

            }



        }




    }
}
