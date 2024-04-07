using Proyecto2.Data.ClasesBase;
using Proyecto2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data.ClasesRepository
{
    public class MensajeRepositorio : BaseRepositorio<Mensaje, int?>
    {
        public MensajeRepositorio(Context db) : base(db)
        {
            lstIncludes.Add("EmisorNavigation");
            lstIncludes.Add("ReceptorNavigation");
            lstIncludes.Add("SubastaNavigation");
        }
    }
}
