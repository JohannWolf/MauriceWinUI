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
        public DbSet<UsoCFDI> UsosCFDI { get; set; }
        public DbSet<RegimenFiscal> RegimenesFiscales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This is only used for design-time operations like migrations
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = System.IO.Path.Combine(path, "maurice.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
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
