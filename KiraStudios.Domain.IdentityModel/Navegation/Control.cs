using KiraStudios.Domain.EntityBase.Implementation;
using KiraStudios.Domain.IdentityModel.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Domain.IdentityModel.Navegation
{
    public class Control : BaseCatalog<int>
    {
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        public bool HasActionValidation { get; set; }

        [Required]
        public bool HasSpecialActionValidation { get; set; }

        public virtual ICollection<SpecialAction> SpecialActions { get; set; }
        public virtual ICollection<RoleControl> RoleControls { get; set; }
    }
}
