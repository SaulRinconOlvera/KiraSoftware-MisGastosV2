using KiraStudios.Domain.EntityBase.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiraStudios.Domain.IdentityModel.Identity
{
    public class UserRole : IdentityUserRole<int>, IBaseEntity<int>, IBaseAuditable
    {
        public UserRole() : base()
        { Enabled = true; }

        [Key]
        public int Id { get; set; }

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
        public virtual Role Role { get; set; }
    }
}
