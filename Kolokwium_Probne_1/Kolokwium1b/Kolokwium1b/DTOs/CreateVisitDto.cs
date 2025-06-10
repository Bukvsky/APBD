namespace Kolokwium1b.DTOs;

public class CreateVisitDto
{
    public int VisitId { get; set; }
    public int ClientId { get; set; }
    public string MechanicLicenceNumber { get; set; }
    public List<CreateVisitServiceDto> Services { get; set; }
}

public class CreateVisitServiceDto
{
    public string ServiceName { get; set; }
    public decimal ServiceFee { get; set; }
}