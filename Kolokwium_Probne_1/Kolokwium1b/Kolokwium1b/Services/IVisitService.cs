using Kolokwium1b.DTOs;

namespace Kolokwium1b.Services;

public interface IVisitService
{
    Task<VisitDetailsDto?> GetVisitDetailsAsync(int visitId);
    Task<bool> CreateVisitAsync(CreateVisitDto createVisitDto);

}