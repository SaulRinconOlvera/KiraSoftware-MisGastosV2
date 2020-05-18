using KiraStudios.Application.ViewModelBase;
using KiraSudios.Application.IdentityViewModel.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiraSudios.Application.IdentityViewModel.Navegation
{
    public class ControlViewModel : BaseCatalogViewModel
    {
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        public bool HasActionValidation { get; set; }

        [Required]
        public bool HasSpecialActionValidation { get; set; }

        public virtual ICollection<RoleControlViewModel> RoleControls { get; set; }
    }
}
