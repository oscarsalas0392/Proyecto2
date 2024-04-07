using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Model
{
    [ModelMetadataType(typeof(MensajeMetadata))]
    public partial class Mensaje
    {
        public class MensajeMetadata
        {

        }
    }
}
