using KiraStudios.Application.ViewModelBase;
using KiraSudios.Application.IdentityViewModel.Navegation;
using System.ComponentModel.DataAnnotations;

namespace KiraSudios.Application.IdentityViewModel.Identity
{
    public class RoleControlViewModel : BaseViewModel
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

        public virtual RoleViewModel Role { get; set; }
        public virtual ControlViewModel Control { get; set; }
    }
}
