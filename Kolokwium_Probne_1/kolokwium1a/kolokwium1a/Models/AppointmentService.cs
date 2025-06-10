using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppointmentServiceModel = kolokwium1a.Models.Appointment;
namespace kolokwium1a.Models;

public class AppointmentService
{
    [Key]
    [Column("appointment_id")]
    public int AppointmentId { get; set; }

    [Key]
    [Column("service_id")]
    public int ServiceId { get; set; }

    [Required]
    [Column("service_fee", TypeName = "decimal(10,2)")]
    public decimal ServiceFee { get; set; }

    [ForeignKey("AppointmentId")]
    public Appointment Appointment { get; set; } = null!;

    [ForeignKey("ServiceId")]
    public Service Service { get; set; } = null!;

    // Parameterless constructor for Entity Framework
    public AppointmentService()
    {
    }

    // Constructor for convenience
    public AppointmentService(int appointmentId, int serviceId, decimal serviceFee)
    {
        AppointmentId = appointmentId;
        ServiceId = serviceId;
        ServiceFee = serviceFee;
    }
}
