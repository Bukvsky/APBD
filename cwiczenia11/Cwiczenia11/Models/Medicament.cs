using System.ComponentModel.DataAnnotations;

namespace Cwiczenia11.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Type { get; set; }
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}