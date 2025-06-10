using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium1a.Models;

public class Patient
{
    [Key]
    [Column("patient_id")]
    public int PatientId { get; set; }
    
    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Required]
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }
}