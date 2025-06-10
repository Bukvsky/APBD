using kolokwium1a.DTOs;
using kolokwium1a.Models;

namespace kolokwium1a.Services;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> GetAppointmentByIdAsync(int id);
    Task<string> CreateAppointmentAsync(CreateAppointmentDto createDto);
}