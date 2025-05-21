using Cwiczenia11.DTOs;
using Cwiczenia11.Models;

namespace Cwiczenia11.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(PrescriptionRequestDTO dto);
    Task<PatientDetailsDTO?> GetPatientDetailsAsync(int idPatient);
}