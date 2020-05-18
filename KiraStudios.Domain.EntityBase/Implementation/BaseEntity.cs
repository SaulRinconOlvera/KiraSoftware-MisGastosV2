using KiraStudios.Domain.EntityBase.Contracts;
using System.ComponentModel.DataAnnotations;

namespace KiraStudios.Domain.EntityBase.Implementation
{
    public abstract class BaseEntity<T> : IBaseEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
