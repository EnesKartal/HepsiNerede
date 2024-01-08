using Microsoft.EntityFrameworkCore;

public class HepsiNeredeDBContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }

    public HepsiNeredeDBContext(DbContextOptions<HepsiNeredeDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}