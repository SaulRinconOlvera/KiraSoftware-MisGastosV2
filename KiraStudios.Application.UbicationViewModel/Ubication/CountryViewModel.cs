using KiraStudios.Application.ViewModelBase;
using System.Collections.Generic;

namespace KiraStudios.Application.UbicationViewModel.Ubication
{
    public class CountryViewModel : BaseCatalogViewModel
    {
        public IEnumerable<StateViewModel> States { get; set; }
    }
}
