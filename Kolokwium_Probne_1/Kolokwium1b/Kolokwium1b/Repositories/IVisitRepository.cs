using Kolokwium1b.Models;

namespace Kolokwium1b.Repositories;

public interface IVisitRepository
{
    Task<Visit?> GetVisitByIdAsync(int visitId);
    Task<bool> VisitExistsAsync(int visitId);
    Task<Visit> CreateVisitAsync(Visit visit);
}