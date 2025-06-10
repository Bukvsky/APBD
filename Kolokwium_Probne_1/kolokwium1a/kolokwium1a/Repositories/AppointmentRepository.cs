using kolokwium1a.Data;
using kolokwium1a.Models;
using Microsoft.EntityFrameworkCore;

namespace kolokwium1a.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentDbContext _context;

    public AppointmentRepository(AppointmentDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a=>a.Patient)
            .Include(a=>a.Doctor)
            .Include(a=>a.AppointmentServices)
            .ThenInclude(aps=>aps.Service)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
    }

    public async Task<bool> AppointmentExistsAsync(int id)
    {
        return await _context.Appointments.AnyAsync(a => a.AppointmentId == id);
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<Doctor?> GetDoctorByPwzAsync(string pwz)
    {
        return await _context.Doctors.FirstOrDefaultAsync(d=>d.PWZ == pwz);
        
    }

    public async Task<Service?> GetServiceByNameAsync(string name)
    {
        return await _context.Services.FirstOrDefaultAsync(s => s.Name == name);
        
    }

    public async Task<bool> CreateAppointmentAsync(Appointment appointment)
    {
        try
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}