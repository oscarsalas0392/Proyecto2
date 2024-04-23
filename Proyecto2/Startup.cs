using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Proyecto2.Data;
using Proyecto2.Data.ClasesRepository;
using Proyecto2.Data.Interfaces;
using Proyecto2.Model;
using Proyecto2.Respuesta;

namespace Proyecto2
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<Context>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("PR"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(
               Configuration.GetConnectionString("PR")));


            services.AddControllersWithViews();
            services.AddRazorPages();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            services.AddSession();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IRepositorio<Artista, int?>, ArtistaRepositorio>();
            services.AddTransient<IRepositorio<CategoriaObra, int?>, CategoriaObraRepositorio>();
            services.AddTransient<IRepositorio<ConfiguracionApp, int?>, ConfiguracionAppRepositorio>();
            services.AddTransient<IRepositorio<DimensionObra, int?>, DimensionObraRepositorio>();
            services.AddTransient<IRepositorio<ImagenObra, int?>, ImagenObraRepositorio>();
            services.AddTransient<IRepositorio<Model.Mensaje, int?>, MensajeRepositorio>();
            services.AddTransient<IRepositorio<Notificacion, int?>, NotificacionRepositorio>();
            services.AddTransient<IRepositorio<ObraArte, int?>, ObraArteRepositorio>();
            services.AddTransient<IRepositorio<Oferta, int?>, OfertaRepositorio>();
            services.AddTransient<IRepositorio<Subasta, int?>, SubastaRepositorio>();
            services.AddTransient<IRepositorio<TipoUsuario, int?>, TipoUsuarioRepositorio>();
            services.AddTransient<IRepositorio<Usuario, int?>, UsuarioRepositorio>();
            services.AddTransient<IRepositorio<Transaccion, int?>, TransaccionRepositorio>();

            services.AddTransient(typeof(ArtistaRepositorio));
            services.AddTransient(typeof(CategoriaObraRepositorio));
            services.AddTransient(typeof(ConfiguracionAppRepositorio));
            services.AddTransient(typeof(DimensionObraRepositorio));
            services.AddTransient(typeof(ImagenObraRepositorio));
            services.AddTransient(typeof(MensajeRepositorio));
            services.AddTransient(typeof(NotificacionRepositorio));
            services.AddTransient(typeof(ObraArteRepositorio));
            services.AddTransient(typeof(OfertaRepositorio));
            services.AddTransient(typeof(SubastaRepositorio));
            services.AddTransient(typeof(TipoUsuarioRepositorio));
            services.AddTransient(typeof(UsuarioRepositorio));
            services.AddTransient(typeof(TransaccionRepositorio));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

    }
}
