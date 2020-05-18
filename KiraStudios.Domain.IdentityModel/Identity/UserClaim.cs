using KiraStudios.Domain.EntityBase.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiraStudios.Domain.IdentityModel.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IBaseEntity<int>, IBaseAuditable
    {
        public UserClaim() : base()
        { Enabled = true; }

        [Key]
        public override int Id { get; set; }

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

        public virtual User User { get; set; }
    }
}
