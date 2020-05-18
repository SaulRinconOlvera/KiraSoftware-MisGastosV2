using KiraStudios.Application.ViewModelBase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Application.UbicationViewModel.Ubication
{
    public class StateViewModel : BaseCatalogViewModel
    {
        [Required]
        public int CountryId { get; set; }
        public virtual CountryViewModel Country { get; set; }
        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
