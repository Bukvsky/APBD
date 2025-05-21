namespace Cwiczenia11.DTOs;

public class PrescriptionRequestDTO
{
    public PatientDTO Patient { get; set; } = null!;
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentPrescriptionDTO> Medicaments { get; set; } = new();
}

public class PatientDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
}

public class MedicamentPrescriptionDTO
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string? Description { get; set; }
}