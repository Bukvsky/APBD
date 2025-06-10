using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace kolokwium1a.Models;

public class Doctor
{
    [Key]
    [Column("doctor_id")]
    public int DoctorId { get; set; }
    
    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Required]
    [Column("PWZ")]
    [MaxLength(7)]
    public string PWZ { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }
}