using KiraStudios.Domain.EntityBase.Implementation;
using KiraStudios.Domain.IdentityModel.Navegation;
using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Domain.IdentityModel.Identity
{
    public class RoleControl : BaseAuditable<int>
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int ControlId { get; set; }

        public bool ApplyActionValidations { get; set; }
        public bool ApplySpecialActionValidations { get; set; }
        public int LevelAccess { get; set; }

        [StringLength(512)]
        public string SpecialActionsValues { get; set; }

        public virtual Role Role { get; set; }
        public virtual Control Control { get; set; }
    }
}
