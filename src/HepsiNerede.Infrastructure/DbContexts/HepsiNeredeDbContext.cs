using HepsiNerede.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Infrastructure.DbContexts
{
    /// <summary>
    /// Represents the database context for the HepsiNerede application.
    /// </summary>
    public class HepsiNeredeDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the set of products in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the set of orders in the database.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the set of campaigns in the database.
        /// </summary>
        public DbSet<Campaign> Campaigns { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HepsiNeredeDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for the database context.</param>
        public HepsiNeredeDbContext(DbContextOptions<HepsiNeredeDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model for the database.
        /// </summary>
        /// <param name="modelBuilder">The model builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
