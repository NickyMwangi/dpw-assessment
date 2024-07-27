using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public partial class BaseEntity
    {
        [Key]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
