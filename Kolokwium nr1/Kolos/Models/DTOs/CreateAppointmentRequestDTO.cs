namespace Kolos.Models.DTOs;

public class CreateAppointmentRequestDTO
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
}

public class appointmentServiceInputDTO
{
    public string Service_Name { get; set; } = string.Empty;
    public decimal Service_Price { get; set; }
}