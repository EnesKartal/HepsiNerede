using System.ComponentModel.DataAnnotations;

namespace HepsiNerede.Data.Entities
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

