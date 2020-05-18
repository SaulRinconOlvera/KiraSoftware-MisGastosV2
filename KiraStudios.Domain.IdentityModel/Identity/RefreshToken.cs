using KiraStudios.Domain.EntityBase.Implementation;

namespace KiraStudios.Domain.IdentityModel.Identity
{
    public class RefreshToken : BaseAuditable<int>
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
    }
}
