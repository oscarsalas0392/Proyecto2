using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Respuesta
{
    public class Respuesta<T>
    {
        public Respuesta(bool estado, Accion accion, bool excepcion = false)
        {
            _estado = estado;
            _accion = accion;
            _excepcion = excepcion;

            if (estado && !_excepcion)
            {
                mensaje = Mensajes.READY;
                return;
            }
            if (excepcion)
            {
                mensaje = Mensajes.EXCEPTION;
                return;
            }

            switch (accion)
            {
                case Accion.actualizar:
                    mensaje = Mensajes.CONCURRENCY_UPDATE;
                    break;

                case Accion.eliminar:
                    mensaje = Mensajes.CONCURRENCY_DELETE;
                    break;

                case Accion.obtener:
                    mensaje = Mensajes.NO_EXISTS;
                    break;
            }
        }

        public bool _estado { get; set; } = false;
        public bool _excepcion { get; set; } = true;
        public Accion _accion { get; set; }

        public Mensaje mensaje = new Mensaje();
        public T? objecto { get; set; }
        public IEnumerable<T>? lista { get; set; }
    }
}
