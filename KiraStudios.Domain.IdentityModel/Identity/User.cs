using KiraStudios.Domain.EntityBase.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiraStudios.Domain.IdentityModel.Identity
{
    public class User : IdentityUser<int>, IBaseEntity<int>, IBaseAuditable
    {
        public User() : base()
        { Enabled = true; }

        public User(string userName) : base(userName)
        { Enabled = true; }

        [Key]
        public override int Id { get; set; }

        [StringLength(128)]
        public string PersonName { get; set; }

        [StringLength(128)]
        public string Alias { get; set; }

        [StringLength(128)]
        public string Avatar { get; set; }

        [StringLength(1024)]
        public string AvatarURL { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreationDate { get; set; }

        [StringLength(50)]
        public string LastModifiedBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime LastModificationDate { get; set; }

        [StringLength(50)]
        public string DeletedBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeletionDate { get; set; }
        public bool Enabled { get; set; }

        public RefreshToken RefreshToken { get; set; }

        [NotMapped]
        public string RolesNames { get; set; }

        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
    }
}
