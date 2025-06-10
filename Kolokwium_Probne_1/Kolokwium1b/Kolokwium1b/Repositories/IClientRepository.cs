namespace Kolokwium1b.Repositories;

public interface IClientRepository
{
    Task<bool> ClientExistsAsync(int clientId);
}