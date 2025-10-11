using Maurice.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maurice.Data.Context
{
    public class MauriceDbContext : DbContext
    {
        public MauriceDbContext() { }

        public MauriceDbContext(DbContextOptions<MauriceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Nomina> Nominas { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indexes (not supported by Data Annotations)
            modelBuilder.Entity<Factura>()
                .HasIndex(e => e.UUID)
                .IsUnique();

            modelBuilder.Entity<Nomina>()
                .HasIndex(e => e.UUID)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
