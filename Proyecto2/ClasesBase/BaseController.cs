using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Proyecto2.Model;

namespace Proyecto2.ClasesBase
{
    public class BaseController : Controller
    {
        public Usuario Usuario()
        {
            string? json = HttpContext.Session.GetString("usuario");

            if (json != null || json == "")
            {
                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);

                if (usuario is null)
                {
                    return new Usuario();
                }
                return usuario;
            }
            return new Usuario();
        }
        public Artista Artista()
        {
            string? json = HttpContext.Session.GetString("artista");

            if (json != null || json == "")
            {
                Artista? usuario = JsonConvert.DeserializeObject<Artista>(json);

                if (usuario is null)
                {
                    return new Artista();
                }
                return usuario;
            }
            return new Artista();
        }

        public void GuardarIntSession(string key, int valor)
        {
            HttpContext.Session.SetInt32(key, valor);
        }
        public void GuardarOjecto(string key, object ojecto)
        {
            string json = JsonConvert.SerializeObject(ojecto);
            HttpContext.Session.SetString(key, json);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string? json = HttpContext.Session.GetString("usuario");
                if (json == null || json == "")
                {
                    context.Result = RedirectToAction("Login", "Acceso");
                    return;
                }

                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);

                if (usuario is null)
                {
                    context.Result = RedirectToAction("Login", "Acceso");
                }
            }
            catch
            {
                context.Result = RedirectToAction("Login", "Acceso");
            }
        }
    }
}
