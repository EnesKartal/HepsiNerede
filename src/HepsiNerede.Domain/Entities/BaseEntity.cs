using System;
using System.ComponentModel.DataAnnotations;

namespace HepsiNerede.Domain.Entities
{
    /// <summary>
    /// Base entity class containing common properties for entities.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of the entity.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
