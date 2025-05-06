namespace Tutorial8.Models.DTOs;

public class CreateTripDTO
{
    public required string Name { get; set; }
    public required List<int> CountryIds { get; set; }
}
