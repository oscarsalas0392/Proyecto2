using Proyecto2.Data.ClasesBase;
using Proyecto2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data.ClasesRepository
{
    public class TransaccionRepositorio : BaseRepositorio<Transaccion, int?>
    {
        public TransaccionRepositorio(Context db) : base(db)
        {
            lstIncludes.Add("UsuarioNavigation");
        }
    }
}
