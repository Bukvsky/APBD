namespace Cwiczenia11.DTOs;

public class GetPatientDetailsResponseDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Birthdate { get; set; } = string.Empty;
    public List<PrescriptionDto> Prescriptions { get; set; } = new List<PrescriptionDto>();
}

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDto Doctor { get; set; } = null!;
    public List<MedicamentDto> Medicaments { get; set; } = new List<MedicamentDto>();
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Dose { get; set; }
}