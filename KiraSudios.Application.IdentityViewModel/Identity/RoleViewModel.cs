using KiraStudios.Application.ViewModelBase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiraSudios.Application.IdentityViewModel.Identity
{
    public class RoleViewModel : BaseViewModel
    {
        [Required]
        [StringLength(128)]
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }
        //public virtual string ConcurrencyStamp { get; set; }

        public virtual ICollection<UserRoleViewModel> Users { get; set; }
    }
}
