using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Domain.EntityBase.Implementation
{
    public class BaseCatalog<T> : BaseAuditable<T>
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        [StringLength(20)]
        public virtual string ShortName { get; set; }
    }
}
