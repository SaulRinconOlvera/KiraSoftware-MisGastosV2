using KiraStudios.Application.ViewModelBase;
using System.ComponentModel.DataAnnotations;

namespace KiraSudios.Application.IdentityViewModel.Identity
{
    public class UserLoginViewModel : BaseViewModel
    {
        [StringLength(128)]
        public string LoginProvider { get; set; }
        [StringLength(128)]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public int UserId { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
