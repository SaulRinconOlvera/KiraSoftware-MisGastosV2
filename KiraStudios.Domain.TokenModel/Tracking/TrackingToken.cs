using KiraStudios.Domain.EntityBase.Implementation;
using System;
using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Domain.TokenModel.Tracking
{
    public class TrackingToken : BaseAuditable<int>
    {
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

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
    }
}
