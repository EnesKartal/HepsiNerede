using System.ComponentModel.DataAnnotations;

namespace HepsiNerede.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

