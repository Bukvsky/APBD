using Kolokwium1b.Models;

namespace Kolokwium1b.Repositories;

public interface IServiceRepository
{
    Task<Service?> GetServiceByNameAsync(string serviceName);
}