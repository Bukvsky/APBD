using System.ComponentModel.DataAnnotations;

public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}