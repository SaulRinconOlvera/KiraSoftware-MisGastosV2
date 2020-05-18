using KiraStudios.Application.ViewModelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace KiraStudios.Application.TokenViewModel.Tracking
{
    public class TrackingTokenViewModel : BaseViewModel
    {
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public DateTime Iat { get; set; }

        [Required]
        public DateTime Nbf { get; set; }

        [Required]
        public DateTime Exp { get; set; }

        [Required]
        public string Audience { get; set; }

        [Required]
        public string Issuer { get; set; }

        public string Token { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
