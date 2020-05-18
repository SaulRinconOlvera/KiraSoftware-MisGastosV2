using KiraStudios.Application.ViewModelBase;

namespace KiraSudios.Application.IdentityViewModel.Identity
{
    public class UserClaimViewModel : BaseViewModel
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int UserId { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
