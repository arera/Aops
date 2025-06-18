using AOps.Domain.Entities;
using AOps.Domain.Entities.Common;
using AOps.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AOps.Infrastructure.Persistence
{
    public class AOpsDbContext : DbContext
    {
        public AOpsDbContext(DbContextOptions<AOpsDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Orglevels> OrganisationLevels => Set<Orglevels>();


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(builder =>
            {
                builder.OwnsOne(c => c.Address, address =>
                {
                    address.Property(a => a.Street).HasMaxLength(100);
                    address.Property(a => a.City).HasMaxLength(100);
                    address.Property(a => a.State).HasMaxLength(100);
                    address.Property(a => a.ZipCode).HasMaxLength(20);
                    address.Property(a => a.Country).HasMaxLength(100);
                });
            });
        }

    }
}
