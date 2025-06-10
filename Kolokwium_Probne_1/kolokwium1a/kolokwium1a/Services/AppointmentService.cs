using kolokwium1a.DTOs;
using kolokwium1a.Models;
using kolokwium1a.Repositories;
using AppointmentServiceModel = kolokwium1a.Models.AppointmentService;

namespace kolokwium1a.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<AppointmentResponseDto?> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _repository.GetAppointmentByIdAsync(appointmentId);
            
            if (appointment == null)
                return null;

            return new AppointmentResponseDto
            {
                Date = appointment.Date,
                Patient = new PatientDto
                {
                    FirstName = appointment.Patient.FirstName,
                    LastName = appointment.Patient.LastName,
                    DateOfBirth = appointment.Patient.DateOfBirth
                },
                Doctor = new DoctorDto
                {
                    DoctorId = appointment.Doctor.DoctorId,
                    Pwz = appointment.Doctor.PWZ
                },
                AppointmentServices = appointment.AppointmentServices.Select(aps => new AppointmentServiceDto
                {
                    Name = aps.Service.Name,
                    ServiceFee = aps.ServiceFee
                }).ToList()
            };
        }

        public async Task<string> CreateAppointmentAsync(CreateAppointmentDto createDto)
        {
            // Check if appointment already exists
            if (await _repository.AppointmentExistsAsync(createDto.AppointmentId))
            {
                return "APPOINTMENT_EXISTS";
            }

            // Check if patient exists
            var patient = await _repository.GetPatientByIdAsync(createDto.PatientId);
            if (patient == null)
            {
                return "PATIENT_NOT_FOUND";
            }

            // Check if doctor exists
            var doctor = await _repository.GetDoctorByPwzAsync(createDto.Pwz);
            if (doctor == null)
            {
                return "DOCTOR_NOT_FOUND";
            }

            // Check if all services exist
            var appointmentServices = new List<AppointmentServiceModel>();
            foreach (var serviceDto in createDto.Services)
            {
                var service = await _repository.GetServiceByNameAsync(serviceDto.ServiceName);
                if (service == null)
                {
                    return "SERVICE_NOT_FOUND";
                }

                appointmentServices.Add(new AppointmentServiceModel
                {
                    AppointmentId = createDto.AppointmentId,
                    ServiceId = service.ServiceId,
                    ServiceFee = serviceDto.ServiceFee
                });
            }

            // Create appointment
            var appointment = new Appointment
            {
                AppointmentId = createDto.AppointmentId,
                PatientId = createDto.PatientId,
                DoctorId = doctor.DoctorId,
                Date = DateTime.Now,
                AppointmentServices = appointmentServices
            };

            var success = await _repository.CreateAppointmentAsync(appointment);
            return success ? "SUCCESS" : "CREATION_FAILED";
        }
    }
}