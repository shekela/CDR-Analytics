using CDR_Analytics.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDR_Analytics.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CDR> CDRs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CDR>()
                .HasIndex(c => c.CallDate)
                .HasDatabaseName("Idx_CallDate");

            modelBuilder.Entity<CDR>()
                .HasIndex(c => c.CallerID)
                .HasDatabaseName("Idx_CallerID");

            modelBuilder.Entity<CDR>()
                .HasIndex(c => c.Recipient)
                .HasDatabaseName("Idx_Recipient");
        }

    }
}
