using KiraStudios.Domain.EntityBase.Implementation;
using System.Collections.Generic;

namespace KiraStudios.Domain.UbicationModel.Ubication
{
    public class Country : BaseCatalog<int>
    {
        public IEnumerable<State> States { get; set; }
    }
}
