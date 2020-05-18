using KiraStudios.Domain.EntityBase.Implementation;
using System.Collections.Generic;

namespace KiraStudios.Domain.UbicationModel.Ubication
{
    public class State : BaseCatalog<int>
    {
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }
}
