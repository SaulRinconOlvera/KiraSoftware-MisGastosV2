using KiraStudios.Application.ViewModelBase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiraSudios.Application.IdentityViewModel.Identity
{
    public class UserViewModel : BaseViewModel
    {
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [StringLength(256)]
        public string NormalizedUserName { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string NormalizedEmail { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(256)]
        public string SecurityStamp { get; set; }

        [StringLength(256)]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [StringLength(128)]
        public string PersonName { get; set; }

        [StringLength(128)]
        public string Alias { get; set; }

        [StringLength(128)]
        public string Avatar { get; set; }

        public RefreshTokenViewModel RefreshToken { get; set; }

        [StringLength(2014)]
        public string AvatarURL { get; set; }
        public string RolesNames { get; set; }
        public virtual ICollection<UserRoleViewModel> Roles { get; set; }
        public virtual ICollection<UserLoginViewModel> Logins { get; set; }
        public virtual ICollection<UserClaimViewModel> Claims { get; set; }
        public string Token { get; set; }
    }
}
