using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Respuesta
{
    public static class Mensajes
    {

        // TODO Evaluar el uso de recursos
        public static Mensaje READY = new Mensaje()
        {
            id = 1,
            nivel = Nivel.Information,
            titulo = "",
            Descripcion = "La acción se ejecutó satisfactoriamente."
        };

        public static Mensaje EXCEPTION = new Mensaje()
        {
            id = 2,
            nivel = Nivel.Exception,
            titulo = "",
            Descripcion = "Se ha presentado un error, vuelva a intertarlo o comuniquese con el administrador del sitio web"
        };

        public static Mensaje CONCURRENCY_DELETE = new Mensaje()
        {
            id = 3,
            nivel = Nivel.Exception,
            titulo = "No fué posible aplicar la eliminación. Los datos fueron eliminados por otro usuario.",
            Descripcion = "La acción fué cancelada debido a que los datos que se intenta eliminar ya no existen en la base de datos y fueron eliminados por otro usuario."
        };

        public static Mensaje CONCURRENCY_UPDATE = new Mensaje()
        {
            id = 4,
            nivel = Nivel.Exception,
            titulo = "No fué posible aplicar la modificación. Los datos fueron modificados por otro usuario.",
            Descripcion = "La acción fué cancelada debido a que los datos que se intenta modificar, fueron previamente por otro usuario."
        };

        public static Mensaje NO_EXISTS = new Mensaje()
        {
            id = 5,
            nivel = Nivel.Warning,
            titulo = "El dato solicitado no existe.",
            Descripcion = "La acción intentó afectar datos que no existen."
        };

        public static Mensaje EXISTS_USERNAME = new Mensaje()
        {
            id = 5,
            nivel = Nivel.Warning,
            titulo = "El dato solicitado no existe.",
            Descripcion = "El nombre de usuario ya existe, por favor digite uno nuevo"
        };


        public static Mensaje EXISTS_EMAIL = new Mensaje()
        {
            id = 5,
            nivel = Nivel.Warning,
            titulo = "El dato solicitado no existe.",
            Descripcion = "El email  ya existe, por favor digite uno nuevo"
        };



    }
}
