using Kolokwium1b.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium1b.Data;

 public class WorkshopDbContext : DbContext
    {
        public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<VisitService> VisitServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Client configuration
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");
                entity.HasKey(e => e.ClientId);
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.DateOfBirth).IsRequired();
            });

            // Mechanic configuration
            modelBuilder.Entity<Mechanic>(entity =>
            {
                entity.ToTable("mechanic");
                entity.HasKey(e => e.MechanicId);
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LicenceNumber).HasMaxLength(14).IsRequired();
            });

            // Service configuration
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("service");
                entity.HasKey(e => e.ServiceId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.BaseFee).IsRequired();
            });

            // Visit configuration
            modelBuilder.Entity<Visit>(entity =>
            {
                entity.ToTable("visit");
                entity.HasKey(e => e.VisitId);
                entity.Property(e => e.Date).IsRequired();
                
                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Visits)
                    .HasForeignKey(d => d.ClientId);
                    
                entity.HasOne(d => d.Mechanic)
                    .WithMany(p => p.Visits)
                    .HasForeignKey(d => d.MechanicId);
            });

            // VisitService configuration
            modelBuilder.Entity<VisitService>(entity =>
            {
                entity.ToTable("visit_service");
                entity.HasKey(e => new { e.ServiceId, e.VisitId });
                entity.Property(e => e.ServiceFee).IsRequired();
                
                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitServices)
                    .HasForeignKey(d => d.VisitId);
                    
                entity.HasOne(d => d.Service)
                    .WithMany(p => p.VisitServices)
                    .HasForeignKey(d => d.ServiceId);
            });
        }
    }
