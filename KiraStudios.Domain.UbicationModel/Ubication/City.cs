using KiraStudios.Domain.EntityBase.Implementation;

namespace KiraStudios.Domain.UbicationModel.Ubication
{
    public class City : BaseCatalog<int>
    {
        public int StateId { get; set; }
        public virtual State State { get; set; }
    }
}
