using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PensionContributionSystem.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
                 : base(options)
        {
            Database.EnsureCreated();
        }
        public async Task SeedDataAsync()
        {
            await DataSeeder.SeedAsync(this);
        }

        public DbSet<Member> Members { get; set; }
            public DbSet<Contribution> Contributions { get; set; }
            public DbSet<Benefit> Benefits { get; set; }
            public DbSet<Employer> Employers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasQueryFilter(m => !m.IsDeleted)
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Contribution>()
                .HasIndex(c => new { c.MemberId, c.ContributionDate });
        }

    
    }
}
