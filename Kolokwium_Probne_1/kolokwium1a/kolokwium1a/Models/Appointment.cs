using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium1a.Models;

public class Appointment
{
    [Key]
    [Column("appointment_id")]
    public int AppointmentId { get; set; }
    
    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }
    
    [Required]
    [Column("doctor_id")]
    public int DoctorId { get; set; }
    
    [Required]
    [Column("date")]
    public DateTime Date { get; set; }
    
    [ForeignKey("PatientId")]
    public Patient Patient { get; set; }
    
    [ForeignKey("DoctorId")]
    public Doctor Doctor { get; set; }
    
    public ICollection<AppointmentService> AppointmentServices { get; set; }
}