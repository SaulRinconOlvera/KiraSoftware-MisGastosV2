namespace KiraSolution.Web.Services.Areas.Authentication.Model
{
    public class TokenInformationViewModel
    {
        public string CurrentJWToken { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
    }
}
