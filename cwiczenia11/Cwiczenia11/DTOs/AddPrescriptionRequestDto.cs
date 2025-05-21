namespace Cwiczenia11.Models;

public class AddPrescriptionRequestDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    public List<AddPrescriptionMedicamentDto> Medicaments { get; set; } = new();
}

public class AddPrescriptionMedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; } = string.Empty;
}