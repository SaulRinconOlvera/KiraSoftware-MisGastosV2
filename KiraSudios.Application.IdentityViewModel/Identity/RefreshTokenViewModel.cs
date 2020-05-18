using KiraStudios.Application.ViewModelBase;
using KiraStudios.CrossCutting.Security.Token;
using System.ComponentModel.DataAnnotations;

namespace KiraSudios.Application.IdentityViewModel.Identity
{
    public class RefreshTokenViewModel : BaseViewModel
    {

        public RefreshTokenViewModel() : base() { }
        public RefreshTokenViewModel(int userId)
        {
            UserId = userId;
            Token = TokenFactory.GenerateToken(64);
        }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }
        public UserViewModel User { get; set; }
    }
}
