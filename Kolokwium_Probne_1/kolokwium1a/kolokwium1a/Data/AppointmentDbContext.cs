namespace kolokwium1a.Data;
using kolokwium1a.Models;
using Microsoft.EntityFrameworkCore;

public class AppointmentDbContext :DbContext
{
    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
    {
        
    }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentService> AppointmentServices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>().ToTable("Appointment");
        modelBuilder.Entity<Patient>().ToTable("Patient");
        modelBuilder.Entity<Doctor>().ToTable("Doctor");
        modelBuilder.Entity<Service>().ToTable("Service");
        modelBuilder.Entity<AppointmentService>().ToTable("Appointment_Service");
        
        modelBuilder.Entity<AppointmentService>()
            .HasKey(e => new { e.ServiceId, e.AppointmentId });

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.DoctorId);
        
        modelBuilder.Entity<AppointmentService>()
            .HasOne(a => a.Appointment)
            .WithMany(a => a.AppointmentServices)
            .HasForeignKey(a => a.AppointmentId);
        
        modelBuilder.Entity<AppointmentService>()
            .HasOne(a => a.Service)
            .WithMany(s => s.AppointmentServices)
            .HasForeignKey(a => a.ServiceId);
        
        base.OnModelCreating(modelBuilder);
    }
}