using Kolokwium1b.Models;

namespace Kolokwium1b.Repositories;

public interface IMechanicRepository
{
    Task<Mechanic?> GetMechanicByLicenceNumberAsync(string licenceNumber);

}