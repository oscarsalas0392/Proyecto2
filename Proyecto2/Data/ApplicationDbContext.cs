using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto2.Model;

namespace Proyecto2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
        {
        }

        public  DbSet<Artista> Artista { get; set; } = default!;

        public  DbSet<CategoriaObra> CategoriaObra { get; set; } = default!;

        public  DbSet<ConfiguracionApp> ConfiguracionApp { get; set; } = default!;

        public  DbSet<DimensionObra> DimensionObra { get; set; } = default!;

        public  DbSet<ImagenObra> ImagenObra { get; set; } = default!;

        public  DbSet<Mensaje> Mensaje { get; set; } = default!;

        public  DbSet<Notificacion> Notificacion { get; set; } = default!;

        public  DbSet<ObraArte> ObraArte { get; set; } = default!;

        public  DbSet<Oferta> Oferta { get; set; } = default!;

        public  DbSet<Subasta> Subasta { get; set; } = default!;

        public  DbSet<TipoUsuario> TipoUsuario { get; set; } = default!;

        public  DbSet<Transaccion> Transaccion { get; set; } = default!;

        public  DbSet<Usuario> Usuario { get; set; } = default!;
     
    }
}
