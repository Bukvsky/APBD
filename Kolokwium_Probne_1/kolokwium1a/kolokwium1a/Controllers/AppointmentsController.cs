using Microsoft.AspNetCore.Mvc;
using kolokwium1a.DTOs;
using kolokwium1a.Services;
using System.ComponentModel.DataAnnotations;

namespace kolokwium1a.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentService.CreateAppointmentAsync(createDto);

            return result switch
            {
                "SUCCESS" => Created($"api/appointments/{createDto.AppointmentId}", null),
                "APPOINTMENT_EXISTS" => Conflict("Appointment with this ID already exists"),
                "PATIENT_NOT_FOUND" => BadRequest("Patient not found"),
                "DOCTOR_NOT_FOUND" => BadRequest("Doctor not found"),
                "SERVICE_NOT_FOUND" => BadRequest("Service not found"),
                _ => StatusCode(500, "Internal server error")
            };
        }
    }
}