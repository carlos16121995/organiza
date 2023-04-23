using System.Diagnostics.CodeAnalysis;

namespace Organiza.Domain.Entities._Base
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity<TKey> 
    {
        public virtual TKey Id { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class BaseEntityDefault<TKey> : BaseEntity<TKey>
             where TKey : struct
    {
        public BaseEntityDefault()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; private set; }
        public bool Active { get; set; } = true;
        public virtual void Delete()
        {
            this.Deleted = true;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
