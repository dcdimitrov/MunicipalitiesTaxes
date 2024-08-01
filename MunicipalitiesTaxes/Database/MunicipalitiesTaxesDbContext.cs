using Microsoft.EntityFrameworkCore;
using MunicipalitiesTaxes.Model;
using System.Reflection.Metadata;

namespace MunicipalitiesTaxes.Database
{
    public class MunicipalitiesTaxesDbContext : DbContext
    {
        public MunicipalitiesTaxesDbContext(DbContextOptions<MunicipalitiesTaxesDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Municipality>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<TaxRecord>()
            .HasKey(v => v.Id);
        }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<TaxRecord> TaxRecords { get; set; }
    }
}
