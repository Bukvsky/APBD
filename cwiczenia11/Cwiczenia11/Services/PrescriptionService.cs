using Microsoft.EntityFrameworkCore;
using Cwiczenia11.Data;
using Cwiczenia11.DTOs;
using Cwiczenia11.Models;
using Cwiczenia11.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia11.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(PrescriptionRequestDTO dto)
        {
            if (dto.Medicaments.Count > 10)
                throw new Exception("Prescription cannot contain more than 10 medicaments.");

            if (dto.DueDate < dto.Date)
                throw new Exception("DueDate must be greater than or equal to Date.");

            var doctor = await _context.Doctors.FindAsync(dto.IdDoctor) ?? throw new Exception("Doctor not found");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p =>
                    p.FirstName == dto.Patient.FirstName && p.LastName == dto.Patient.LastName &&
                    p.Birthdate == dto.Patient.Birthdate);

            patient ??= new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthdate = dto.Patient.Birthdate
            };

            var medicamentEntities = await _context.Medicaments
                .Where(m => dto.Medicaments.Select(dm => dm.IdMedicament).Contains(m.IdMedicament))
                .ToListAsync();

            if (medicamentEntities.Count != dto.Medicaments.Count)
                throw new Exception("Some medicaments not found");

            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                Doctor = doctor,
                Patient = patient
            };

            foreach (var med in dto.Medicaments)
            {
                prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = med.IdMedicament,
                    Dose = med.Dose,
                    Details = med.Description
                });
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        
        public async Task<PatientDetailsDTO?> GetPatientDetailsAsync(int idPatient)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

            if (patient == null) return null;

            return new PatientDetailsDTO
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate,
                Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate).Select(p => new PrescriptionDTO
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDTO
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Details
                    }).ToList()
                }).ToList()
            };
        }
    }
}