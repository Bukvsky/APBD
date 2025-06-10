namespace Kolokwium1b.DTOs;

public class VisitDetailsDto
{
    public DateTime Date { get; set; }
    public ClientDto Client { get; set; }
    public MechanicDto Mechanic { get; set; }
    public List<VisitServiceDto> VisitServices { get; set; }
    
}

public class ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class MechanicDto
{
    public int MechanicId { get; set; }
    public string LicenceNumber { get; set; }
}

public class VisitServiceDto
{
    public string Name { get; set; }
    public decimal ServiceFee { get; set; }
}