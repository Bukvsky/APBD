using kolokwium1a.Models;

namespace kolokwium1a.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetAppointmentByIdAsync(int appointment_id);
    Task<bool> AppointmentExistsAsync(int appointment_id);
    Task<Patient?> GetPatientByIdAsync(int patient_id);
    Task<Doctor?> GetDoctorByPwzAsync(string PWZ);
    Task<Service?> GetServiceByNameAsync(string name);
    Task<bool> CreateAppointmentAsync(Appointment appointment);
}