using System.ComponentModel.DataAnnotations;
public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
