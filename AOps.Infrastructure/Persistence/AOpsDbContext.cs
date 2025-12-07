using AOps.Domain.Entities;
using AOps.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AOps.Infrastructure.Persistence
{
    public class AOpsDbContext : DbContext
    {
        public AOpsDbContext(DbContextOptions<AOpsDbContext> options) : base(options) { }
       // public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Orglevels> OrganisationLevels => Set<Orglevels>();
        public DbSet<VendorMaster> VendorMaster => Set<VendorMaster>();
        public DbSet<VendorContract> VendorContract => Set<VendorContract>();

        public DbSet<VehicleMaster> VehicleMaster => Set<VehicleMaster>();

        public DbSet<VehicleDocument> VehicleDocument => Set<VehicleDocument>();

        public DbSet<CustomerContract> CustomerContract => Set<CustomerContract>();

        public DbSet<CustomerSite>CustomerSite => Set<CustomerSite>();
        public DbSet<SiteVehicleAssignment>SiteVehicleAssignment => Set<SiteVehicleAssignment>();

        public DbSet<CustomerLogin> CustomerLogin => Set<CustomerLogin>();

        public DbSet<CustomerTickets>CustomerTickets => Set<CustomerTickets>();

        public DbSet<SiteExpenses> SiteExpenses => Set<SiteExpenses>();
        public DbSet<EmployeeMaster>Employeemaster => Set<EmployeeMaster>();

        public DbSet<SiteEmployeeAssignment>SiteEmployeeAssignment => Set<SiteEmployeeAssignment>();

        public DbSet<AOESiteMapping> AOESiteMapping => Set<AOESiteMapping>();

        public DbSet<EmployeeContract>EmployeeContracts => Set<EmployeeContract>();


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

            modelBuilder.Entity<VendorMaster>()
            .HasKey(v => v.VendorId);

            modelBuilder.Entity<VehicleMaster>()
            .HasKey(vm => vm.VehicleId);


            modelBuilder.Entity<VendorContract>(entity =>
            {
                entity.HasOne(vc => vc.Vendor)
                      .WithMany() // or .WithMany(v => v.Contracts) if using navigation in VendorMaster
                      .HasForeignKey(vc => vc.VendorId);

                entity.Property(vc => vc.ContractValue)
                      .HasPrecision(18, 4);
            });

            modelBuilder.Entity<CustomerContract>(entity =>
            {
                entity.HasOne(cc => cc.Customer)
                      .WithMany()
                      .HasForeignKey(cc => cc.CustomerId)
                      .HasPrincipalKey(c => c.UserId); // 👈 explicitly link FK to UserId

                entity.Property(cc => cc.ContractValue)
                      .HasPrecision(18, 4);
            });

            modelBuilder.Entity<VehicleDocument>()
             .HasOne(doc => doc.Vehicle)
            .WithMany(vm => vm.Documents)
            .HasForeignKey(doc => doc.VehicleId);

            modelBuilder.Entity<CustomerSite>()
            .HasKey(cs => cs.SiteId);

            modelBuilder.Entity<EmployeeMaster>()
           .HasKey(e => e.EmployeeId);


            modelBuilder.Entity<SiteVehicleAssignment>()
           .HasOne(sva => sva.CustomerSite)
           .WithMany(site => site.SiteVehicleAssignments)
           .HasForeignKey(sva => sva.SiteId);

            modelBuilder.Entity<SiteVehicleAssignment>()
                .HasOne(sva => sva.Vehicle)
                .WithMany()
                .HasForeignKey(sva => sva.VehicleId);

           modelBuilder.Entity<SiteEmployeeAssignment>()
          .HasOne(sva => sva.CustomerSite)
          .WithMany(site => site.SiteEmployeeAssignments)
          .HasForeignKey(sva => sva.SiteId);

            modelBuilder.Entity<SiteEmployeeAssignment>()
               .HasOne(sva => sva.Employee)
               .WithMany()
               .HasForeignKey(sva => sva.EmployeeId);


            modelBuilder.Entity<AOESiteMapping>()
            .HasOne(m => m.CustomerSite)
            .WithMany() // or .WithMany(c => c.AOESiteMappings) if needed
            .HasForeignKey(m => m.SiteId)
            .HasPrincipalKey(cs => cs.SiteId) // ✅ map to SiteId instead of ID
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AOESiteMapping>()
        .HasOne(m => m.Orglevels)
        .WithMany()
        .HasForeignKey(m => m.OrgEmployeeId)
        .HasPrincipalKey(o => o.UserID) // Explicit FK mapping to UserId
        .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
