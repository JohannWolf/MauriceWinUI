using Maurice.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maurice.Data.Context
{
    internal class MauriceDbContext : DbContext
    {
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Nomina> Nominas { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var dbPath = System.IO.Path.Combine(localFolder, "maurice.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

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
