using KiraStudios.Application.ViewModelBase;
using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Application.UbicationViewModel.Ubication
{
    public class CityViewModel : BaseCatalogViewModel
    {
        [Required]
        public int StateId { get; set; }
        public virtual StateViewModel State { get; set; }
    }
}
