namespace Cwiczenia11.DTOs;

public class PatientDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public List<PrescriptionDTO> Prescriptions { get; set; } = new();
}

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDTO Doctor { get; set; } = null!;
    public List<MedicamentDTO> Medicaments { get; set; } = new();
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = string.Empty;
}

public class MedicamentDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Dose { get; set; }
    public string? Description { get; set; }
}