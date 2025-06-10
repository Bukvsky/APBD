using System.ComponentModel.DataAnnotations;

namespace kolokwium1a.DTOs;

public class CreateAppointmentDto
{
    [Required]
    public int AppointmentId { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    
    [Required]
    [MaxLength(7)]
    public string Pwz { get; set; } = string.Empty;
    
    [Required]
    public List<ServiceRequestDto> Services { get; set; }
    
}

public class ServiceRequestDto
{
    [Required]
    [MaxLength(100)]
    public string ServiceName { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal ServiceFee { get; set; }
    
    
}