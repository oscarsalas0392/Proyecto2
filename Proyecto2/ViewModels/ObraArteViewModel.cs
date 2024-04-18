using Proyecto2.Model;

namespace Proyecto2.ViewModels
{
    public class ObraArteViewModel
    {
        public ObraArte obraArte { get; set; }
        public DimensionObra dimensionObra { get; set; }
        public List<ImagenObra> listImagenesObra { get; set; }
        public List<ImagenObra> listImagenesObraAgregar { get; set; }
        public List<ImagenObra> listImagenesObraEliminar { get; set; }
    }
}
